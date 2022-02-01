using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pet.Services.ProductAPI.Models.Dto;
using Pet.Services.ProductAPI.Repository;
using Microsoft.AspNetCore.Authorization;

namespace Pet.Services.ProductAPI.Controllers
{
   [ApiController]
    public class ProductApiController : ControllerBase
    {

        protected  ResponseDto _response;
        protected  IProductRepository _repository;


        public ProductApiController(IProductRepository repository)
        {
            _repository = repository;
            _response = new ResponseDto();
        }
       
        [Route("api/ProductApi/GetProducts")]
     
        [HttpGet]
    

        public  async Task<object> Get()
        {

            try
            {
                var Result = await _repository.Getproducts();
                _response.Result = Result;

            }
            catch (Exception e)
            {

                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.Message };
            }

            return _response;

        }
        [Route("api/ProductApi/GetProducts/{ProductId:int}")]
        [HttpGet]
      
        public async Task<object> Get(int ProductId)
        {


            


            try
            {
                 
                var Result = await _repository.GetProductById(ProductId);
                _response.Result = Result;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.Message };

            }

            return _response;
       
        }



        [HttpPost]
        [Authorize]
        [Route("api/ProductApi/GetProducts/")]
        public async Task<Object> Post([FromBody] ProductDto Product)
        {
            try
            {
                var Result = await _repository.CreateUpdateProduct(Product);
                _response.Result = Result;
            }
            catch (Exception e)
            {

                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.Message };
            }


            return _response;

        }
        [HttpPut]
        [Authorize]
        [Route("api/ProductApi/GetProducts/")]
        public async Task<Object> Put([FromBody] ProductDto Product)
        {
            try
            {
                var Result = await _repository.CreateUpdateProduct(Product);
                _response.Result = Result;
            }
            catch (Exception e)
            {

                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.Message };
            }


            return _response;

        }


  
        [HttpDelete]
        [Authorize]
        //[Authorize(Roles = "Admin")]
        [Route("{Id}")]
        public async Task<Object> Delete( int Id)
        {
            try
            {
                var Result = await _repository.DeleteProduct( Id);
                _response.Result = Result;
            }
            catch (Exception e)
            {

                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.Message };
            }


            return _response;

        }




    }
}
