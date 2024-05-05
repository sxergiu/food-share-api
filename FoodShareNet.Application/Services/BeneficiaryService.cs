using FoodShareNet.Application.Exceptions;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNet.Application.Services
{
    public class BeneficiaryService : IBeneficiaryService
    {
        private readonly IFoodShareDbContext _context;

        public BeneficiaryService(IFoodShareDbContext context)
        {
            _context = context;
        }

        public async Task<Beneficiary> CreateBeneficiaryAsync(Beneficiary beneficiary)
        {
            _context.Beneficiaries.Add(beneficiary);
            await _context.SaveChangesAsync();
            return beneficiary;
        }

        public async Task<bool> DeleteBeneficiaryAsync(int id)
        {
            var beneficiary = await _context.Beneficiaries.FindAsync(id);

            if(beneficiary == null) { throw new NotFoundException("beneficiary", id);  }

            _context.Beneficiaries.Remove(beneficiary);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<Beneficiary> EditBeneficiaryAsync(int id, Beneficiary EditBeneficiary)
        {
            var beneficiary = await _context.Beneficiaries
            .FirstOrDefaultAsync(b => b.Id == id);

            if( beneficiary is null ) { throw new NotFoundException("beneficiary",id); }

            beneficiary.Name = EditBeneficiary.Name;
            beneficiary.Address = EditBeneficiary.Address;
            beneficiary.Capacity = EditBeneficiary.Capacity;
            beneficiary.CityId = EditBeneficiary.CityId;

            await _context.SaveChangesAsync();
            return beneficiary;
        }

        public async Task<IList<Beneficiary>> GetAllBeneficiariesAsync()
        {
            var beneficiaries = await _context.Beneficiaries
                .Include(b => b.City)
                .Select(b => new Beneficiary
                {
                    Id = b.Id,
                    Name = b.Name,
                    Address = b.Address,
                    Capacity = b.Capacity,
                    City = b.City
                }).ToListAsync();
            return beneficiaries;
        }

        public async Task<Beneficiary> GetBeneficiaryByIdAsync(int id)
        {
            var beneficiary = await _context.Beneficiaries
                .Select(b => new Beneficiary
                {
                    Id = b.Id,
                    Name = b.Name,
                    Address = b.Address,
                    Capacity = b.Capacity,
                    City = b.City
                }).FirstOrDefaultAsync(a => a.Id == id);

            if( beneficiary is null) { throw new NotFoundException("beneficiary"); }

            return beneficiary;
        }
    }
}
