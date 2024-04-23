using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodShareNet.Domain.Entities;
using FoodShareNet.Repository.Data;
using FoodShareNetAPI.DTO.Order;
using OrderStatusEnum = FoodShareNet.Domain.Enums.OrderStatus;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly FoodShareNetDbContext _context;
    public OrderController(FoodShareNetDbContext context)
    {
        _context = context;
    }

    [ProducesResponseType(type: typeof(OrderDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<OrderDTO>> CreateOrder(CreateOrderDTO createOrderDTO)
    {
        if( !ModelState.IsValid )
        {
            return BadRequest(ModelState);
        }

        var donation = await _context.Donations
            .FirstOrDefaultAsync( d => d.Id == createOrderDTO.DonationId );

        if( donation == null )
        {
            return NotFound($"Invalid Donation ID: {createOrderDTO.DonationId}. ");
        }

        if( donation.Quantity < createOrderDTO.Quantity )
        {
            return BadRequest("Requested quantity exceeds available donation quantity" +
                $" for donation with ID: {donation.Id}");
        }

        donation.Quantity -= createOrderDTO.Quantity;

        var order = new Order
        {
            BeneficiaryId = createOrderDTO.BeneficiaryId,
            DonationId = createOrderDTO.DonationId,
            CourierId = createOrderDTO.CourierId,
            OrderStatusId = createOrderDTO.OrderStatusId,
            CreationDate = createOrderDTO.CreationDate
        };

        _context.Add(order);
        await _context.SaveChangesAsync();

        var orderEntityDTO = new OrderDetailsDTO
        {
            Id = order.Id,
            BeneficiaryId = order.BeneficiaryId,
            DonationId = order.DonationId,
            CourierId = order.CourierId,
            OrderStatusId = order.OrderStatusId,
            CreationDate = order.CreationDate
        };

        return Ok(orderEntityDTO);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public async Task<ActionResult<OrderDTO>> GetOrder(int id)
    {
        var orderDTO = await _context.Orders
            .Select(o => new OrderDTO
            {
                Id = o.Id,
                BeneficiaryId = o.BeneficiaryId,
                CourierId = o.CourierId,
                DonationId = o.DonationId,
                OrderStatusId = o.OrderStatusId,
                CreationDate = o.CreationDate,
                BeneficiaryName = o.Beneficiary.Name,
                CourierName = o.Courier.Name,
                OrderStatusName = o.OrderStatus.Name,
                DonationProduct = o.Donation.Product.Name

            }).FirstOrDefaultAsync(a => a.Id == id);

        if( orderDTO == null )
        {
            return NotFound();
        }

        return Ok(orderDTO);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPatch()]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDTO updateStatusDTO)
    {
        if( orderId != updateStatusDTO.OrderId ) { return BadRequest(); }

        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId );

        if( order == null ) { return NotFound();  }

        if (!_context.orderStatuses.Any(s => s.Id == updateStatusDTO.NewStatusId))
        {
            return NotFound($"Status with ID {updateStatusDTO.NewStatusId} not found.");
        }

        order.OrderStatusId = updateStatusDTO.NewStatusId;
        order.DeliveryDate = updateStatusDTO.NewStatusId == (int)OrderStatusEnum.Delivered ? DateTime.UtcNow : order.DeliveryDate;

        await _context.SaveChangesAsync();
        return Ok(order);
    }
}
