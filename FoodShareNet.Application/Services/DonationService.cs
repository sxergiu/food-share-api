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

    public class DonationService : IDonationService
    {
        private readonly IFoodShareDbContext _context;

        public DonationService(IFoodShareDbContext context)
        {
            _context = context;
        }

        public async Task<Donation> CreateDonationAsync(Donation donation)
        {
            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();
            return donation;
        }

        public async Task<Donation> GetDonationByIdAsync(int id)
        {
            var donation = await _context.Donations
            .Select(d => new Donation
            {
                Id = d.Id,
                ProductId = d.ProductId,
                Quantity = d.Quantity,
                ExpirationDate = d.ExpirationDate,
                DonorId = d.DonorId,
                StatusId = d.StatusId
            }).FirstOrDefaultAsync(d => d.Id == id);

            if( donation == null ) { throw new NotFoundException("donation"); }

            return donation;

        }

        public async Task<IList<Donation>> GetDonationsByCityIdAsync(int cityId)
        {
            var donations = await _context.Donations
            .Where(d => d.Donor.CityId == cityId)
            .Select(d => new Donation
            {
                Id = d.Id,
                ProductId = d.ProductId,
                Quantity = d.Quantity,
                ExpirationDate = d.ExpirationDate,
                StatusId = d.StatusId,
                DonorId = d.DonorId
            }).ToListAsync();

            if( donations.Count == 0 ) { throw new NotFoundException("donationsByCity");  }

            return donations;
        }
    }
}
