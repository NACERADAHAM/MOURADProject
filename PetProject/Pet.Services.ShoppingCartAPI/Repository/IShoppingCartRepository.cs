using Pet.Services.ShoppingCartAPI.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Services.ShoppingCartAPI.Repository
{
    public interface IShoppingCartRepository
    {


        Task<CartDto>GetCartByUserId (string  UserId);
        Task<CartDto> CreateorUpdate(CartDto CartDto);

        Task<bool> Remove(int  CartDetailsId);
        Task<bool> ApplyCoupon(string UserId, string CouponCode);
        Task<bool> RemoveCoupon(string UserId);
        Task<bool> ClearCart(string UserId);

    }
}
