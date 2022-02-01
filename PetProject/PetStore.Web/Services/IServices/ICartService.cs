using PetStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Web.Services.IServices
{
    public interface  ICartService
    {
        Task<T> GetCartByUserIdAsync<T>(string UserId, string Token = null);
        Task<T> AddToCartAsync<T>(CartDto CartDto, string Token = null);
        Task<T> UpdateToCartAsync<T>(CartDto CartDto, string Token = null);
        Task<T> RemoveFromCartAsync<T>(int CartdetailId, string Token = null);
        Task<T> ApplyCoupon<T>(CartDto CartDto, string Token = null);
        Task<T> RemoveCoupon<T>(string UserId, string Token = null);
        Task<T> checkout<T>(CartHeaderDto cartheader, string Token = null);

    }
}
