using prismodPurchase.Src.Application.DTOs.Purchasing;
using prismodPurchase.Src.Application.DTOs.Common;

namespace prismodPurchase.Src.Application.Interfaces;

public interface ISupplierService
{
    Task<IEnumerable<SupplierDto>> GetByCompanyAsync(string companyCen);
    Task<SupplierDto> CreateAsync(string companyCen, CreateSupplierDto dto);
    Task UpdateAsync(string supplierCen, UpdateSupplierDto dto);
}

public interface IPurchaseOrderService
{
    Task<PurchaseOrderSummaryDto> CreateAsync(string companyCen, CreatePurchaseOrderDto dto);
    Task UpdateAsync(string orderCen, CreatePurchaseOrderDto dto);
    Task<PurchaseOrderDetailDto?> GetByCenAsync(string orderCen);
    Task<PagedResultDto<PurchaseOrderListDto>> GetPagedByCompanyAsync(string companyCen, PurchaseOrderQueryFilters filters);
    Task<PurchaseOrderConfirmationDto> ConfirmAsync(string companyCen, string orderCen);
}
