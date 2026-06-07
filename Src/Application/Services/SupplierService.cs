using prismodPurchase.Src.Application.DTOs.Purchasing;
using prismodPurchase.Src.Application.Interfaces;
using prismodPurchase.Src.Domain.Entities;
using prismodPurchase.Src.Infraestructure.Persistence.Interfaces;

namespace prismodPurchase.Src.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public SupplierService(ISupplierRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<SupplierDto>> GetByCompanyAsync(string companyCen)
    {
        var suppliers = await _repository.GetByCompanyCenAsync(companyCen);
        return suppliers.Select(s => new SupplierDto { SupplierCen = s.SupplierCen, Name = s.Name });
    }

    public async Task<SupplierDto> CreateAsync(string companyCen, CreateSupplierDto dto)
    {
        var supplier = new Supplier(companyCen, dto.Name);
        await _repository.AddAsync(supplier);
        await _unitOfWork.SaveChangesAsync();
        return new SupplierDto { SupplierCen = supplier.SupplierCen, Name = supplier.Name };
    }

    public async Task UpdateAsync(string supplierCen, UpdateSupplierDto dto)
    {
        var supplier = await _repository.GetByCenAsync(supplierCen);
        if (supplier == null) throw new KeyNotFoundException("Supplier not found");

        typeof(Supplier).GetProperty(nameof(Supplier.Name))?.SetValue(supplier, dto.Name);

        await _repository.UpdateAsync(supplier);
        await _unitOfWork.SaveChangesAsync();
    }
}
