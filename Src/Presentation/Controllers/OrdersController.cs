using Microsoft.AspNetCore.Mvc;
using fastinventoryPurchase.Src.Application.DTOs.Purchasing;
using fastinventoryPurchase.Src.Application.DTOs.Common;
using fastinventoryPurchase.Src.Application.Interfaces;

namespace fastinventoryPurchase.Src.Presentation.Controllers;

[ApiController]
[Route("api/purchases/companies/{companyCen}/orders")]
public class OrdersController : ControllerBase
{
    private readonly IPurchaseOrderService _orderService;
    public OrdersController(IPurchaseOrderService orderService) => _orderService = orderService;

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<PurchaseOrderListDto>>> GetAll(
        string companyCen,
        [FromQuery] short? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] bool sortDescending = true)
    {
        var filters = new PurchaseOrderQueryFilters
        {
            Status = status,
            Page = page,
            PageSize = pageSize,
            SortDescending = sortDescending
        };
        var result = await _orderService.GetPagedByCompanyAsync(companyCen, filters);
        return Ok(result);
    }

    [HttpGet("{orderCen}")]
    public async Task<ActionResult<PurchaseOrderDetailDto>> GetByCen(string companyCen, string orderCen)
    {
        var order = await _orderService.GetByCenAsync(orderCen);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<PurchaseOrderSummaryDto>> Create(string companyCen, CreatePurchaseOrderDto dto)
    {
        var result = await _orderService.CreateAsync(companyCen, dto);
        return CreatedAtAction(nameof(GetByCen), new { companyCen, orderCen = result.OrderCen }, result);
    }

    [HttpPut("{orderCen}")]
    public async Task<ActionResult<PurchaseOrderSummaryDto>> Update(string companyCen, string orderCen, CreatePurchaseOrderDto dto)
    {
        var result = await _orderService.UpdateAsync(orderCen, dto);
        return Ok(result);
    }

    [HttpPost("{orderCen}/confirm")]
    public async Task<ActionResult<PurchaseOrderConfirmationDto>> Confirm(string companyCen, string orderCen)
    {
        var result = await _orderService.ConfirmAsync(companyCen, orderCen);
        return Ok(result);
    }
}
