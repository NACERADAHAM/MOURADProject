using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Pet.Services.ShoppingCartAPI.Models;
using Pet.Services.ShoppingCartAPI.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pet.Services.ShoppingCartAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly HttpClient _httpclient;

        public CouponRepository(HttpClient httpclient)
        {
            _httpclient = httpclient;
       
      
        }
        public async Task<CouponDto> getcoupon(string couponname)
        {

            var respone = await _httpclient.GetAsync($"api/Coupon/{couponname}");
            var apicontent = await respone.Content.ReadAsStringAsync();
            var ResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apicontent);
            if (ResponseDto.IsSuccess)
            {

        return  JsonConvert.DeserializeObject<CouponDto>(ResponseDto.Result.ToString());
            }
            return new CouponDto() { };
        }
    }
}
