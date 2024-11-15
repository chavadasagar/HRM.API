using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.API.Application.Catalog.Products;
public interface IProductService : IScopedService
{
    Task<long> GenerateProductCode();
}
