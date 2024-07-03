using Microsoft.AspNetCore.Mvc;
using FoodShareNet.Domain.Entities;
using FoodShareNetAPI.DTO.Order;
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
        var order = await _orderService.GetOrderAsync(id);

        var orderDTO = new OrderDTO
        {
            Id = order.Id,
            BeneficiaryId = order.BeneficiaryId,
            BeneficiaryName = order.Beneficiary.Name,
            CourierId = order.CourierId,
            CourierName = order.Courier.Name,
            DonationId = order.DonationId,
            DonationProduct = order.Donation.Product.Name,
            CreationDate = order.CreationDate,
            DeliveryDate = order.DeliveryDate,
            OrderStatusId = order.OrderStatusId,
            OrderStatusName = order.OrderStatus.Name,
            Quantity = order.Quantity
        };

        return Ok(orderDTO);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPatch()]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDTO updateStatusDTO)
    {
        if( orderId != updateStatusDTO.OrderId ) { return BadRequest("Mismatched Id!"); }

        var order = await _orderService.UpdateOrderStatusAsync(orderId, (FoodShareNet.Domain.Enums.OrderStatus) updateStatusDTO.NewStatusId);

        return Ok(order);
    }
}
