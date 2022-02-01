using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetStore.Web.Services.IServices;
using PetStore.Web.Models;
using PetStore.Web.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace PetStore.Web.Controllers
{
    public class ProductController : Controller
    {
        IProductService _productService;


       public  ProductController( IProductService product) {


            _productService = product;
                
                }
        [Authorize]
        public async Task<ActionResult> ProductIndex()
        {

            List<ProductDto> listproduct = new List<ProductDto>();
            var accesstoken = await HttpContext.GetTokenAsync("access_token");
            var Responseresult = await _productService.GetAllProductAsync<ResponseDto>(accesstoken);
            if(Responseresult.IsSuccess==true && Responseresult!=null)
                     listproduct = JsonConvert.DeserializeObject<List<ProductDto>>(Responseresult.Result.ToString());

            return View(listproduct);
        }
        [Authorize]
        public async Task<ActionResult> ProductCreate()
        {

            

             return  View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProductCreate(ProductDto product)
        {


            if (ModelState.IsValid)
            {
                var accesstoken = await HttpContext.GetTokenAsync("access_token");
                var Responseresult = await _productService.CreateProductAsync<ResponseDto>(product,accesstoken);
                if (Responseresult.IsSuccess == true && Responseresult != null)
                    return RedirectToAction(nameof(ProductIndex));

                
            }
                return View(product);
        }
        public async Task<ActionResult>  EditProduct (int ProductId)
        {
            if (ModelState.IsValid)
            {
                var accesstoken = await HttpContext.GetTokenAsync("access_token");
                var Responseresult = await _productService.GetProductByIdAsync<ResponseDto>(ProductId,accesstoken);
                if (Responseresult.IsSuccess == true && Responseresult != null)
                {
                    var product = JsonConvert.DeserializeObject<ProductDto>(Responseresult.Result.ToString());
                    return View(product);
                }

            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProduct(ProductDto product)
        {
            if (ModelState.IsValid)
            {
                var accesstoken = await HttpContext.GetTokenAsync("access_token");
                var Responseresult = await _productService.CreateProductAsync<ResponseDto>(product,accesstoken);
                if (Responseresult.IsSuccess == true && Responseresult != null)
                    return RedirectToAction(nameof(ProductIndex));


            }
            return View(product);
        }
        [Authorize]
        public async Task<ActionResult> DeleteProduct(int ProductId)
        {
            if (ModelState.IsValid)
            {
                var accesstoken = await HttpContext.GetTokenAsync("access_token");
                var Responseresult = await _productService.GetProductByIdAsync<ResponseDto>(ProductId,accesstoken);
                if (Responseresult.IsSuccess == true )
                {
                    var product = JsonConvert.DeserializeObject<ProductDto>(Responseresult.Result.ToString());
                    return View(product);
                }

            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteProduct(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                var accesstoken = await HttpContext.GetTokenAsync("access_token");
                var Responseresult = await _productService.DeleteProductAsync<ResponseDto>(model.ProductId,accesstoken);
                if (Responseresult.IsSuccess == true && Responseresult != null)
                    return RedirectToAction(nameof(ProductIndex));


            }
            return View();
        }
    }
}
