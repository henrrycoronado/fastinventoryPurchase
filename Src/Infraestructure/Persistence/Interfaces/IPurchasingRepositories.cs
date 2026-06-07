using fastinventoryPurchase.Src.Domain.Entities;
using fastinventoryPurchase.Src.Application.DTOs.Common;

namespace fastinventoryPurchase.Src.Infraestructure.Persistence.Interfaces;

public interface ISupplierRepository
{
    Task<Supplier?> GetByCenAsync(string supplierCen);
    Task<IEnumerable<Supplier>> GetByCompanyCenAsync(string companyCen);
    Task AddAsync(Supplier supplier);
    Task UpdateAsync(Supplier supplier);
}

public interface IPurchaseOrderRepository
{
    Task<PurchaseOrder?> GetByCenAsync(string orderCen);
    Task<(IEnumerable<PurchaseOrder> Items, int TotalCount)> GetPagedByCompanyCenAsync(string companyCen, PurchaseOrderQueryFilters filters);
    Task AddAsync(PurchaseOrder order);
    Task UpdateAsync(PurchaseOrder order);
    Task ReplaceItemsAsync(string orderCen, IEnumerable<PurchaseOrderItem> newItems);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
