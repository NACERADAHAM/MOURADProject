using Pet.Services.CouponAPI.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Services.CouponAPI.Repository
{
    public interface ICouponRepository
    {

        Task<CouponDto> GetCouponCode(string couponCode);
    }
}
