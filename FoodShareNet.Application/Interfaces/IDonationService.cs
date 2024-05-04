using FoodShareNet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNet.Application.Interfaces
{
    public interface IDonationService
    {
        Task<Donation> CreateDonationAsync(Donation donation);
        Task<Donation> GetDonationByIdAsync(int id);
        Task<IList<Donation>> GetDonationsByCityNameAsync(string cityName);
    }
}
