using fastinventoryPurchase.Src.Application.DTOs.Purchasing;
using fastinventoryPurchase.Src.Application.DTOs.Common;
using fastinventoryPurchase.Src.Application.Interfaces;
using fastinventoryPurchase.Src.Domain.Entities;
using fastinventoryPurchase.Src.Infraestructure.Persistence.Interfaces;
using fastinventoryPurchase.Src.Infraestructure.ExternalServices;

namespace fastinventoryPurchase.Src.Application.Services;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly IPurchaseOrderRepository _repository;
    private readonly IInventoryClient _inventoryClient;
    private readonly IUnitOfWork _unitOfWork;

    public PurchaseOrderService(IPurchaseOrderRepository repository, IInventoryClient inventoryClient, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _inventoryClient = inventoryClient;
        _unitOfWork = unitOfWork;
    }

    public async Task<PurchaseOrderSummaryDto> CreateAsync(string companyCen, CreatePurchaseOrderDto dto)
    {
        var order = new PurchaseOrder(companyCen, dto.WarehouseCen, dto.SupplierCen);
        foreach (var item in dto.Items)
        {
            order.AddItem(item.ProductCen, item.Quantity);
        }

        await _repository.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return new PurchaseOrderSummaryDto { OrderCen = order.OrderCen, Status = order.Status };
    }

    public async Task UpdateAsync(string orderCen, CreatePurchaseOrderDto dto)
    {
        var order = await _repository.GetByCenAsync(orderCen);
        if (order == null) throw new KeyNotFoundException("Order not found");
        if (order.Status != 0) throw new InvalidOperationException("Only orders with status CREATED can be modified");

        typeof(PurchaseOrder).GetProperty(nameof(PurchaseOrder.WarehouseCen))?.SetValue(order, dto.WarehouseCen);
        typeof(PurchaseOrder).GetProperty(nameof(PurchaseOrder.SupplierCen))?.SetValue(order, dto.SupplierCen);

        var newItems = dto.Items.Select(i => new PurchaseOrderItem(i.ProductCen, i.Quantity)).ToList();
        
        await _repository.UpdateAsync(order);
        await _repository.ReplaceItemsAsync(orderCen, newItems);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PurchaseOrderDetailDto?> GetByCenAsync(string orderCen)
    {
        var order = await _repository.GetByCenAsync(orderCen);
        if (order == null) return null;

        return new PurchaseOrderDetailDto
        {
            OrderCen = order.OrderCen,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            ConfirmedAt = order.ConfirmedAt,
            SupplierCen = order.SupplierCen,
            WarehouseCen = order.WarehouseCen,
            Items = order.Items.Select(i => new PurchaseOrderDetailItemDto
            {
                ProductCen = i.ProductCen,
                Quantity = (int)i.Quantity
            }).ToList()
        };
    }

    public async Task<PagedResultDto<PurchaseOrderListDto>> GetPagedByCompanyAsync(string companyCen, PurchaseOrderQueryFilters filters)
    {
        var (items, totalCount) = await _repository.GetPagedByCompanyCenAsync(companyCen, filters);

        return new PagedResultDto<PurchaseOrderListDto>
        {
            Items = items.Select(o => new PurchaseOrderListDto
            {
                OrderCen = o.OrderCen,
                Status = o.Status,
                CreatedAt = o.CreatedAt,
                ConfirmedAt = o.ConfirmedAt,
                SupplierCen = o.SupplierCen,
                ItemCount = o.Items.Count
            }),
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / filters.PageSize),
            CurrentPage = filters.Page
        };
    }

    public async Task<PurchaseOrderConfirmationDto> ConfirmAsync(string companyCen, string orderCen)
    {
        var order = await _repository.GetByCenAsync(orderCen);
        if (order == null) throw new KeyNotFoundException("Order not found");
        if (order.Status == 1) throw new InvalidOperationException("Order already confirmed");

        var stockRequest = new StockValidationRequestDto
        {
            WarehouseCen = order.WarehouseCen,
            Source = "PURCHASE_ORDER",
            ReferenceCen = order.OrderCen,
            Items = order.Items.Select(i => new StockValidationItemDto
            {
                ProductCen = i.ProductCen,
                Quantity = i.Quantity
            }).ToList()
        };

        var inventoryResult = await _inventoryClient.IncreaseStockAsync(companyCen, stockRequest);
        if (!inventoryResult)
        {
            throw new InvalidOperationException("Failed to update stock in Inventory system");
        }

        order.Confirm();
        await _repository.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return new PurchaseOrderConfirmationDto
        {
            OrderCen = order.OrderCen,
            Status = order.Status,
            ConfirmedAt = order.ConfirmedAt!.Value
        };
    }
}
