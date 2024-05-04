using FoodShareNet.Application.Exceptions;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNet.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IFoodShareDbContext _context;

        public OrderService(IFoodShareDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
        
            var donation = await _context.Donations
                .FirstOrDefaultAsync(d => d.Id == order.DonationId);

            if (donation == null)
            {
                throw new NotFoundException("donation",donation.Id.ToString());
            }

            if (donation.Quantity < order.Quantity)
            {
                throw new OrderException("Donation quantity inssuficient");
            }

            donation.Quantity -= order.Quantity;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            var order = await _context.Orders
                .Include( o => o.Beneficiary)
                .Include( o => o.Courier )
                .Include( o => o.Donation )
                .Include( o => o.Donation.Product)
                .Include( o => o.OrderStatus )
                .FirstOrDefaultAsync(a => a.Id == id);

            if(order == null)
            {
                throw new NotFoundException("order", order.Id.ToString());
            }

            return order;
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, Domain.Enums.OrderStatus orderStatus)
        {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId );

                if( order == null ) { throw new NotFoundException("order", order.Id.ToString()); }

                if (!_context.orderStatuses.Any(s => s.Id == (int) orderStatus))
                {
                   throw new NotFoundException("order status", (int) orderStatus);
                }

                order.OrderStatusId = (int)orderStatus;
                order.DeliveryDate = ((int)orderStatus) == (int)Domain.Enums.OrderStatus.Delivered ? DateTime.UtcNow : order.DeliveryDate;

                await _context.SaveChangesAsync();
                return true;         
        }
    }
}
