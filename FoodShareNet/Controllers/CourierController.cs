﻿using Microsoft.AspNetCore.Mvc;
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

        var courierDTOS = couriers.Select(c => new CourierDTO
        {
            Id = c.Id,
            Name = c.Name,
            Price = c.Price
        }).ToList();
           
        return Ok(courierDTOS);
    }

}
