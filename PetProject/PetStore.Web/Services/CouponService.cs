using PetStore.Web.Models;
using PetStore.Web.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetStore.Web.Services.IServices;
using System.Net.Http;

namespace PetStore.Web.Services
{
    public class CouponService : BaseService, ICouponService
    {
        private readonly IHttpClientFactory _httpClient;


        public CouponService(IHttpClientFactory client) : base(client)
        {


            _httpClient = client;

        }
        public async  Task<T> GetCoupon<T>(string  couponCode, string Token = null)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponApiBase + "api/Coupon/" + couponCode,

                AccessToken = Token
            }); ;
        }
    }
}
