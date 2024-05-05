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
        Task<Beneficiary> EditBeneficiaryAsync(int id, Beneficiary beneficiary);
        Task<bool> DeleteBeneficiaryAsync(int id);


    }
}
