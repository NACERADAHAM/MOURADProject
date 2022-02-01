using AutoMapper;
using Pet.Services.CouponAPI.Models;
using Pet.Services.CouponAPI.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Services.CouponAPI
{
    public class Mappingconfig
    {
        public static MapperConfiguration RegisterMaps()
        {

            var mappingconfig = new MapperConfiguration(config => {

                config.CreateMap<Coupon, CouponDto>().ReverseMap();

            });
            return mappingconfig;
        }
    }
}
