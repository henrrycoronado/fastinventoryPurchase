using Microsoft.AspNetCore.Mvc;
using fastinventoryPurchase.Src.Application.DTOs.Purchasing;
using fastinventoryPurchase.Src.Application.Interfaces;

namespace fastinventoryPurchase.Src.Presentation.Controllers;

[ApiController]
[Route("api/purchases/companies/{companyCen}/suppliers")]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;
    public SuppliersController(ISupplierService supplierService) => _supplierService = supplierService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupplierDto>>> GetByCompany(string companyCen)
    {
        var suppliers = await _supplierService.GetByCompanyAsync(companyCen);
        return Ok(suppliers);
    }

    [HttpPost]
    public async Task<ActionResult<SupplierDto>> Create(string companyCen, CreateSupplierDto dto)
    {
        var supplier = await _supplierService.CreateAsync(companyCen, dto);
        return CreatedAtAction(nameof(GetByCompany), new { companyCen }, supplier);
    }

    [HttpPut("{supplierCen}")]
    public async Task<ActionResult<SupplierDto>> Update(string supplierCen, UpdateSupplierDto dto)
    {
        var supplier = await _supplierService.UpdateAsync(supplierCen, dto);
        return Ok(supplier);
    }
}