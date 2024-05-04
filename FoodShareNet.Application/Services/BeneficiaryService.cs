using FoodShareNet.Application.Interfaces;
using FoodShareNet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNet.Application.Services
{
    public class BeneficiaryService : IBeneficiaryService
    {
        public Task<Beneficiary> CreateBeneficiaryAsync(Beneficiary beneficiary)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteBeneficiaryAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditBeneficiaryAsync(int id, Beneficiary beneficiary)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Beneficiary>> GetAllBeneficiariesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Beneficiary> GetBeneficiaryByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
