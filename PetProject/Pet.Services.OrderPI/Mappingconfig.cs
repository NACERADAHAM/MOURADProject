using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Pet.Services.OrderAPI.Models;


namespace Pet.Services.OrderAPI
{
    
    public class Mappingconfig
    {
        public static  MapperConfiguration RegisterMaps()
        {

            var mappingconfig = new MapperConfiguration(config => {
                //config.CreateMap<ProductDto,Product>().ReverseMap();
                //config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
                //config.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
               

                //config.CreateMap<Cart, CartDto>().ReverseMap();


            });
            return mappingconfig;
        }
    }
}
