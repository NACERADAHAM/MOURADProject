using PetStore.Web.Models;
using PetStore.Web.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PetStore.Web.Services
{
    public class ProductService : BaseService, IProductService
    {

        private readonly  IHttpClientFactory _httpClient;


        public ProductService(IHttpClientFactory client):base(client)
        {


            _httpClient = client;

        }
        public async  Task<T> CreateProductAsync<T>(ProductDto ProductDto,string token)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Url = SD.ProductApiBase + "api/ProductApi/GetProducts",
                Data=ProductDto,
                AccessToken=token
                
            }); ;
        }

        public async Task<T> DeleteProductAsync<T>(int Id, string token)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.ProductApiBase + ""+Id,
            
                AccessToken = token

            }); ;
        }

        public async  Task<T> GetAllProductAsync<T>(string token)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductApiBase + "api/ProductApi/GetProducts",

                AccessToken = token

            }); ;
        }

        public async Task<T> GetProductByIdAsync<T>(int id, string token)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductApiBase + "api/ProductApi/GetProducts/"+id,

                AccessToken = token

            }); ;
        }

        public async Task<T> UpdateProductAsync<T>(ProductDto ProductDto, string token)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.PUT,
                Url = SD.ProductApiBase + "api/ProductApi/GetProducts",
                Data=ProductDto,
                AccessToken = token

            }); ;
        }
    }
}
