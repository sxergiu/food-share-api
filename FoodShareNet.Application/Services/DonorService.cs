using FoodShareNet.Application.Interfaces;
using FoodShareNet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNet.Application.Services
{
    public class DonorService : IDonorService
    {
        public Task<Donor> CreateDonorAsync(Donor donor)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteDonorAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditDonorAsync(int id, Donor donor)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Donor>> GetAllDonorsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Donor> GetDonorByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
