using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Pet.Services.ProductAPI.Models;
using Pet.Services.ProductAPI.Models.Dto;

namespace Pet.Services.ProductAPI
{
    public class Mappingconfig
    {
        public static  MapperConfiguration RegisterMaps()
        {

            var mappingconfig = new MapperConfiguration(config => {
                config.CreateMap<ProductDto, Product>().ReverseMap(); 
            
            
            
            
            });
            return mappingconfig;
        }
    }
}
