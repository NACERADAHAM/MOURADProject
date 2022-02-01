using PetStore.Web.Models;
using PetStore.Web.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PetStore.Web.Services
{
    public class CartService : BaseService, ICartService
    {
        private readonly IHttpClientFactory _httpClient;


        public CartService(IHttpClientFactory client) : base(client)
        {


            _httpClient = client;

        }

        public async Task<T> AddToCartAsync<T>(CartDto CartDto, string Token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Url = SD.ShoppingApiBase + "api/CartApi/CreateCart",
                Data = CartDto,
                AccessToken = Token
            }); ;
        }

        public async Task<T> ApplyCoupon<T>(CartDto CartDto, string Token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Url = SD.ShoppingApiBase + "api/CartApi/applyCoupon",
                Data = CartDto,
                AccessToken = Token
            }); ;
        }

        public async Task<T> GetCartByUserIdAsync<T>(string UserId, string Token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ShoppingApiBase + "api/CartApi/GetCart/"+UserId,

                AccessToken = Token
            }) ; ;
        }

        public async Task<T> RemoveCoupon<T>(string UserId, string Token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Url = SD.ShoppingApiBase + "api/CartApi/RemoveCoupon",
                Data = UserId,
                AccessToken = Token
            }); ;
        }

        public async  Task<T> RemoveFromCartAsync<T>(int  CartdetailId, string Token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Url = SD.ShoppingApiBase + "api/CartApi/Delete",
                Data=CartdetailId,
                AccessToken = Token
            }); ;
        }

       

        public async Task<T> UpdateToCartAsync<T>(CartDto CartDto, string Token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Url = SD.ShoppingApiBase + "api/CartApi/UpdateCart",
                Data = CartDto,
                AccessToken = Token
            }); ;
        }
        public async Task<T> checkout<T>(CartHeaderDto cartheader, string Token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Url = SD.ShoppingApiBase + "api/CartApi/Checkout",
                Data = cartheader,
                AccessToken = Token
            }); ;
        }
    }
}
