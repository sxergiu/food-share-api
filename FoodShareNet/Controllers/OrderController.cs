using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using FoodShareNetAPI.DTO.Order;
using OrderStatusEnum = FoodShareNet.Domain.Enums.OrderStatus;
using FoodShareNet.Application.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [ProducesResponseType(type: typeof(OrderDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<OrderDTO>> CreateOrder([FromBody] CreateOrderDTO createOrderDTO)
    {

        var order = new Order
        {
            BeneficiaryId = createOrderDTO.BeneficiaryId,
            DonationId = createOrderDTO.DonationId,
            CourierId = createOrderDTO.CourierId,
            OrderStatusId = createOrderDTO.OrderStatusId,
            CreationDate = createOrderDTO.CreationDate.Date,
            Quantity = createOrderDTO.Quantity
        };

        var createdOrder = await _orderService.CreateOrderAsync(order);

        var orderDetails = new OrderDetailsDTO
        {
            Id = createdOrder.Id,
            BeneficiaryId = createdOrder.BeneficiaryId,
            CourierId = createdOrder.CourierId,
            DonationId = createdOrder.DonationId,
            CreationDate = createdOrder.CreationDate,
            DeliveryDate = createdOrder.DeliveryDate,
            OrderStatusId = createdOrder.OrderStatusId,
            Quantity = createdOrder.Quantity
        };

        return Ok(orderDetails);

    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public async Task<ActionResult<OrderDTO>> GetOrder(int id)
    {
        var orderDTO = await _orderService.GetOrderAsync(id);

        var order = new OrderDTO
        {
            Id = orderDTO.Id,
            BeneficiaryId = orderDTO.BeneficiaryId,
            BeneficiaryName = orderDTO.Beneficiary.Name,
            CourierId = orderDTO.CourierId,
            CourierName = orderDTO.Courier.Name,
            DonationId = orderDTO.DonationId,
            DonationProduct = orderDTO.Donation.Product.Name,
            CreationDate = orderDTO.CreationDate,
            DeliveryDate = orderDTO.DeliveryDate,
            OrderStatusId = orderDTO.OrderStatusId,
            OrderStatusName = orderDTO.OrderStatus.Name,
            Quantity = orderDTO.Quantity
        };

        return Ok(order);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPatch()]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDTO updateStatusDTO)
    {
        if( orderId != updateStatusDTO.OrderId ) { return BadRequest(); }

        var order = await _orderService.UpdateOrderStatusAsync(orderId, (FoodShareNet.Domain.Enums.OrderStatus) updateStatusDTO.NewStatusId);

        return Ok(order);
    }
}
