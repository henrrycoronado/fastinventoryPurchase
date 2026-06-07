using PrismodPurchase.Src.Application.DTOs.Purchasing;
using PrismodPurchase.Src.Application.DTOs.Common;

namespace PrismodPurchase.Src.Application.Interfaces;

public interface ISupplierService
{
    Task<IEnumerable<SupplierDto>> GetByCompanyAsync(string companyCen);
}

public interface IPurchaseOrderService
{
    Task<PurchaseOrderSummaryDto> CreateAsync(string companyCen, CreatePurchaseOrderDto dto);
    Task<PurchaseOrderDetailDto?> GetByCenAsync(string orderCen);
    Task<PagedResultDto<PurchaseOrderListDto>> GetPagedByCompanyAsync(string companyCen, PurchaseOrderQueryFilters filters);
    Task<PurchaseOrderConfirmationDto> ConfirmAsync(string companyCen, string orderCen);
}
