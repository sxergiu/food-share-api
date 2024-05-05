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
    public class DonorService : IDonorService
    {
        private readonly IFoodShareDbContext _context;

        public DonorService(IFoodShareDbContext context)
        {
            _context = context;
        }

        public async Task<Donor> CreateDonorAsync(Donor donor)
        {
            _context.Donors.Add(donor);
            await _context.SaveChangesAsync();
            return donor;
        }

        public async Task<bool> DeleteDonorAsync(int id)
        {
            var donor = await _context.Donors.FirstOrDefaultAsync(d => d.Id == id);

            if (donor == null) { throw new NotFoundException("donor", id); }

            _context.Donors.Remove(donor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Donor> EditDonorAsync(int id, Donor editDonor)
        {
            var donor = await _context.Donors.FirstOrDefaultAsync(d => d.Id == id);

            if( donor  == null ) { throw new NotFoundException("donor",id); }

            donor.Name = editDonor.Name;
            donor.Address = editDonor.Address;
            donor.CityId = editDonor.CityId;

            await _context.SaveChangesAsync();
            return donor;
        }

        public async Task<IList<Donor>> GetAllDonorsAsync()
        {
            var donors = await _context.Donors
            .Select(d => new Donor
            {
                Id = d.Id,
                Name = d.Name,
                Address = d.Address,
                CityId = d.CityId,
                City = d.City,
                Donations = d.Donations.Select(donation => new Donation
                {
                    Id = donation.Id,
                    Product = donation.Product,
                    Quantity = donation.Quantity,
                    Status = donation.Status,
                    ExpirationDate = donation.ExpirationDate
                }).ToList()

            }).ToListAsync();

            if (donors.Count == 0)
            {
                throw new NotFoundException("donors");
            }
            return donors;
        }

        public async Task<Donor> GetDonorByIdAsync(int id)
        {
            var donor = await _context.Donors.Select(d => new Donor
            {
                Id = d.Id,
                Name = d.Name,
                Address = d.Address,
                City = d.City,
                Donations = d.Donations.Select(donation => new Donation
                {
                    Id = donation.Id,
                    Product = donation.Product,
                    ExpirationDate = donation.ExpirationDate,
                    Status = donation.Status,
                    Quantity = donation.Quantity
                }).ToList()

            }).FirstOrDefaultAsync(a => a.Id == id);

            if ( donor == null ) { throw new NotFoundException("donor");  }

            return donor;
        }
    }
}
