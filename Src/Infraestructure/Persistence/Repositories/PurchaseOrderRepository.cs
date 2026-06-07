using Microsoft.EntityFrameworkCore;
using PrismodPurchase.Src.Domain.Entities;
using PrismodPurchase.Src.Application.DTOs.Common;
using PrismodPurchase.Src.Infraestructure.Persistence.Interfaces;
using PrismodPurchase.Src.Infraestructure.Persistence.Models;

namespace PrismodPurchase.Src.Infraestructure.Persistence.Repositories;

public class PurchaseOrderRepository : IPurchaseOrderRepository
{
    private readonly ApplicationDbContext _dbContext;
    public PurchaseOrderRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<PurchaseOrder?> GetByCenAsync(string orderCen)
    {
        var model = await _dbContext.PurchaseOrders.Include(o => o.Items).AsNoTracking().FirstOrDefaultAsync(o => o.OrderCen == orderCen);
        return model == null ? null : MapToDomain(model);
    }

    public async Task<(IEnumerable<PurchaseOrder> Items, int TotalCount)> GetPagedByCompanyCenAsync(string companyCen, PurchaseOrderQueryFilters filters)
    {
        var query = _dbContext.PurchaseOrders.Include(o => o.Items).Where(o => o.CompanyCen == companyCen);

        if (filters.Status.HasValue)
        {
            query = query.Where(o => o.Status == filters.Status.Value);
        }

        if (filters.SortDescending)
        {
            query = query.OrderByDescending(o => o.CreatedAt);
        }
        else
        {
            query = query.OrderBy(o => o.CreatedAt);
        }

        var totalCount = await query.CountAsync();
        var models = await query.Skip((filters.Page - 1) * filters.PageSize).Take(filters.PageSize).AsNoTracking().ToListAsync();

        return (models.Select(MapToDomain), totalCount);
    }

    public async Task AddAsync(PurchaseOrder order)
    {
        await _dbContext.PurchaseOrders.AddAsync(MapToModel(order));
    }

    public async Task UpdateAsync(PurchaseOrder order)
    {
        var model = await _dbContext.PurchaseOrders.FirstOrDefaultAsync(o => o.OrderCen == order.OrderCen);
        if (model != null)
        {
            model.Status = order.Status;
            model.ConfirmedAt = order.ConfirmedAt;
            _dbContext.PurchaseOrders.Update(model);
        }
    }

    private static PurchaseOrder MapToDomain(PurchaseOrderModel model)
    {
        var order = new PurchaseOrder(model.CompanyCen, model.WarehouseCen, model.SupplierCen);
        typeof(PurchaseOrder).GetProperty(nameof(PurchaseOrder.OrderCen))?.SetValue(order, model.OrderCen);
        typeof(PurchaseOrder).GetProperty(nameof(PurchaseOrder.Status))?.SetValue(order, model.Status);
        typeof(PurchaseOrder).GetProperty(nameof(PurchaseOrder.CreatedAt))?.SetValue(order, model.CreatedAt);
        typeof(PurchaseOrder).GetProperty(nameof(PurchaseOrder.ConfirmedAt))?.SetValue(order, model.ConfirmedAt);

        foreach (var item in model.Items)
        {
            order.AddItem(item.ProductCen, item.Quantity);
            var addedItem = order.Items.Last();
            typeof(PurchaseOrderItem).GetProperty(nameof(PurchaseOrderItem.ItemCen))?.SetValue(addedItem, item.ItemCen);
        }
        return order;
    }

    private static PurchaseOrderModel MapToModel(PurchaseOrder entity)
    {
        var model = new PurchaseOrderModel
        {
            OrderCen = entity.OrderCen,
            CompanyCen = entity.CompanyCen,
            WarehouseCen = entity.WarehouseCen,
            SupplierCen = entity.SupplierCen,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt,
            ConfirmedAt = entity.ConfirmedAt
        };

        foreach (var item in entity.Items)
        {
            model.Items.Add(new PurchaseOrderItemModel
            {
                ItemCen = item.ItemCen,
                OrderCen = entity.OrderCen,
                ProductCen = item.ProductCen,
                Quantity = item.Quantity
            });
        }
        return model;
    }
}
