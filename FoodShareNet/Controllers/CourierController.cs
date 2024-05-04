using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using FoodShareNetAPI.DTO.Beneficiary;
using FoodShareNetAPI.DTO.Order;
using OrderStatusEnum = FoodShareNet.Domain.Enums.OrderStatus;
using FoodShareNetAPI.DTO.Courier;
using FoodShareNet.Application.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class CourierController : ControllerBase
{
    private readonly ICourierService _courierService;
    public CourierController(ICourierService courierService)
    {
        _courierService = courierService;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<CourierDTO>>> GetAllAsync()
    {
        var couriers = await _courierService.GetCouriersAsync();
        return Ok(couriers);
    }

}
