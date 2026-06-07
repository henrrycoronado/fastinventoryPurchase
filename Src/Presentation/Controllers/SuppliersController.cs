using Microsoft.AspNetCore.Mvc;
using prismodPurchase.Src.Application.DTOs.Purchasing;
using prismodPurchase.Src.Application.Interfaces;

namespace prismodPurchase.Src.Presentation.Controllers;

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
    public async Task<IActionResult> Update(string supplierCen, UpdateSupplierDto dto)
    {
        await _supplierService.UpdateAsync(supplierCen, dto);
        return NoContent();
    }
}
