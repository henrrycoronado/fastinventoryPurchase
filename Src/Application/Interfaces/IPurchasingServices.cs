using fastinventoryPurchase.Src.Application.DTOs.Purchasing;
using fastinventoryPurchase.Src.Application.DTOs.Common;

namespace fastinventoryPurchase.Src.Application.Interfaces;

public interface ISupplierService
{
    Task<IEnumerable<SupplierDto>> GetByCompanyAsync(string companyCen);
    Task<SupplierDto> CreateAsync(string companyCen, CreateSupplierDto dto);
    Task<SupplierDto> UpdateAsync(string supplierCen, UpdateSupplierDto dto);
}

public interface IPurchaseOrderService
{
    Task<PurchaseOrderSummaryDto> CreateAsync(string companyCen, CreatePurchaseOrderDto dto);
    Task<PurchaseOrderSummaryDto> UpdateAsync(string orderCen, CreatePurchaseOrderDto dto);
    Task<PurchaseOrderDetailDto?> GetByCenAsync(string orderCen);
    Task<PagedResultDto<PurchaseOrderListDto>> GetPagedByCompanyAsync(string companyCen, PurchaseOrderQueryFilters filters);
    Task<PurchaseOrderConfirmationDto> ConfirmAsync(string companyCen, string orderCen);
}