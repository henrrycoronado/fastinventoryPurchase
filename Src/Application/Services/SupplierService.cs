using PrismodPurchase.Src.Application.DTOs.Purchasing;
using PrismodPurchase.Src.Application.Interfaces;
using PrismodPurchase.Src.Infraestructure.Persistence.Interfaces;

namespace PrismodPurchase.Src.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _repository;
    public SupplierService(ISupplierRepository repository) => _repository = repository;

    public async Task<IEnumerable<SupplierDto>> GetByCompanyAsync(string companyCen)
    {
        var suppliers = await _repository.GetByCompanyCenAsync(companyCen);
        return suppliers.Select(s => new SupplierDto { SupplierCen = s.SupplierCen, Name = s.Name });
    }
}
