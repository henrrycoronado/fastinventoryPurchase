using Microsoft.EntityFrameworkCore;
using fastinventoryPurchase.Src.Domain.Entities;
using fastinventoryPurchase.Src.Infraestructure.Persistence.Interfaces;
using fastinventoryPurchase.Src.Infraestructure.Persistence.Models;

namespace fastinventoryPurchase.Src.Infraestructure.Persistence.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly ApplicationDbContext _dbContext;
    public SupplierRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<Supplier?> GetByCenAsync(string supplierCen)
    {
        var model = await _dbContext.Suppliers.AsNoTracking().FirstOrDefaultAsync(s => s.SupplierCen == supplierCen);
        return model == null ? null : MapToDomain(model);
    }

    public async Task<IEnumerable<Supplier>> GetByCompanyCenAsync(string companyCen)
    {
        var models = await _dbContext.Suppliers.AsNoTracking().Where(s => s.CompanyCen == companyCen).ToListAsync();
        return models.Select(MapToDomain);
    }

    public async Task AddAsync(Supplier supplier)
    {
        await _dbContext.Suppliers.AddAsync(MapToModel(supplier));
    }

    public async Task UpdateAsync(Supplier supplier)
    {
        var model = await _dbContext.Suppliers.FirstOrDefaultAsync(s => s.SupplierCen == supplier.SupplierCen);
        if (model != null)
        {
            model.Name = supplier.Name;
            _dbContext.Suppliers.Update(model);
        }
    }

    private static Supplier MapToDomain(SupplierModel model)
    {
        var supplier = new Supplier(model.CompanyCen, model.Name);
        typeof(Supplier).GetProperty(nameof(Supplier.SupplierCen))?.SetValue(supplier, model.SupplierCen);
        return supplier;
    }

    private static SupplierModel MapToModel(Supplier entity) => new()
    {
        SupplierCen = entity.SupplierCen,
        CompanyCen = entity.CompanyCen,
        Name = entity.Name
    };
}
