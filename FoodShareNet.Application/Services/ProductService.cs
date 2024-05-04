using FoodShareNet.Application.Interfaces;
using FoodShareNet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNet.Application.Services
{
    public class ProductService : IProductService
    {
        public Task<IList<Product>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
