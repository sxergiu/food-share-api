using FoodShareNet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNet.Application.Interfaces
{
    public interface IBeneficiaryService
    {
        Task<IList<Beneficiary>> GetAllBeneficiariesAsync();
        Task<Beneficiary> GetBeneficiaryByIdAsync(int id);
        Task<Beneficiary> CreateBeneficiaryAsync(Beneficiary beneficiary);
        Task<bool> EditBeneficiaryAsync(int id, Beneficiary beneficiary);
        Task<bool> DeleteBeneficiaryAsync(string id);


    }
}
