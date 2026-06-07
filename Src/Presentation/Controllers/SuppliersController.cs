using Microsoft.AspNetCore.Mvc;
using PrismodPurchase.Src.Application.DTOs.Purchasing;
using PrismodPurchase.Src.Application.Interfaces;

namespace PrismodPurchase.Src.Presentation.Controllers;

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
}
