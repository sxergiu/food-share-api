using FoodShareNet.Application.Interfaces;
using FoodShareNet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNet.Application.Services
{
    public class DonationService : IDonationService
    {
        public Task<Donation> CreateDonationAsync(Donation donation)
        {
            throw new NotImplementedException();
        }

        public Task<Donation> GetDonationByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Donation>> GetDonationsByCityNameAsync(string cityName)
        {
            throw new NotImplementedException();
        }
    }
}
