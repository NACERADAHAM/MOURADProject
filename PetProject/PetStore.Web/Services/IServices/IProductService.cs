using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetStore.Web.Models;
namespace PetStore.Web.Services.IServices
{
    public interface IProductService
    {

        Task<T> GetAllProductAsync<T>(string token);
        Task<T> GetProductByIdAsync<T>(int id, string token);
        Task<T> CreateProductAsync<T>(ProductDto ProductDto, string token);
        Task<T> UpdateProductAsync<T>(ProductDto ProductDto, string token);
        Task<T> DeleteProductAsync<T>(int Id, string token);

    }
}
