﻿using FoodShareNet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNet.Application.Interfaces
{
    public interface IProductService
    {
        Task<IList<Product>> GetAllProductsAsync();

    }
}
