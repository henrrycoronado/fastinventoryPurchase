using Microsoft.AspNetCore.Mvc;

namespace PrismodPurchase.Src.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            service = "prismodPurchase",
            status = "ok",
            timestampUtc = DateTime.UtcNow
        });
    }
}
