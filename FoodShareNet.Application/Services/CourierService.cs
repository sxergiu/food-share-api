using FoodShareNet.Application.Exceptions;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodShareNet.Application.Services
{
    public class CourierService : ICourierService
    {
        private readonly IFoodShareDbContext _context;

        public CourierService(IFoodShareDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Courier>> GetCouriersAsync()
        {
            var couriers = await _context.Couriers.Select(c => new Courier
            {
                Id = c.Id,
                Name = c.Name,
                Price = c.Price
            }).ToListAsync();

            if (couriers.Count == 0)
                throw new NotFoundException("couriers");

            return couriers;
        }
    }
}
