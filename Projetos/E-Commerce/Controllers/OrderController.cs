using Microsoft.AspNetCore.Mvc;
using E_Commerce.Services;
using Order.DTOs;

[ApiController]
[Route("api/[controller]")]

public class OrderController : ControllerBase
{
    private readonly IOrderService _service;
    public OrderController(IOrderService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetAll()
    {
        var orders = await _service.GetAll();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponse>> GetById(Guid id)
    {
        var order = await _service.GetById(id);
        if (order is null) return NotFound();
        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create(OrderRequest request)
    {
        try
        {
            var order = await _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<OrderResponse>> Update(Guid id, OrderRequest request)
    {
        try
        {
            var order = await _service.Update(id, request);
            if (order is null) return NotFound();
            return Ok(order);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            var deleted = await _service.Delete(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }
}