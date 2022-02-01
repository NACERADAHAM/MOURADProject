using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PetStore.Web.Models;
using PetStore.Web.Services.IServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Web.Controllers
{
    public class HomeController : Controller
    {
        IProductService _productService;
        private readonly ILogger<HomeController> _logger;
        ICartService _cartService;
        
        public HomeController(IProductService product, ILogger<HomeController> logger, ICartService Cartservice)
        {

            _logger = logger;
            _productService = product;
            _cartService = Cartservice;

        }
        public async Task<IActionResult> Index()
        {
            List<ProductDto> listproduct = new List<ProductDto>();
            var accesstoken = await HttpContext.GetTokenAsync("access_token");
            var Responseresult = await _productService.GetAllProductAsync<ResponseDto>(accesstoken);
            if (Responseresult.IsSuccess == true && Responseresult != null)
                listproduct = JsonConvert.DeserializeObject<List<ProductDto>>(Responseresult.Result.ToString());

            return View(listproduct);
        }
        [Authorize]
        public async Task<IActionResult> Details(int ProductId)
        {
            if (ModelState.IsValid)
            {
                var accesstoken = await HttpContext.GetTokenAsync("access_token");
                var Responseresult = await _productService.GetProductByIdAsync<ResponseDto>(ProductId, accesstoken);
                if (Responseresult.IsSuccess == true && Responseresult != null)
                {
                    var product = JsonConvert.DeserializeObject<ProductDto>(Responseresult.Result.ToString());
                    return View(product);
                }

            }
            return NotFound();
        }
        [Authorize]
        [HttpPost]
        [ActionName("Details")]
        public async Task<IActionResult> DetailsPost(ProductDto ProductDto)
        {
            CartDto cart = new()
            {
                CartHeader =new CartHeaderDto { UserId=User.Claims.Where(u => u.Type =="sub")?.FirstOrDefault()?.Value }
            };

            List<CartDetailsDto> Cartdetails = new();

            CartDetailsDto cartdetails = new CartDetailsDto() {

                
                ProductId = ProductDto.ProductId,
                Count=ProductDto.Count

            };
            var accesstoken = await HttpContext.GetTokenAsync("access_token");
            var Responseresult = await _productService.GetProductByIdAsync<ResponseDto>(ProductDto.ProductId, accesstoken);
            if (Responseresult.IsSuccess == true && Responseresult != null)
            {
                cartdetails.Product = JsonConvert.DeserializeObject<ProductDto>(Responseresult.Result.ToString());
               
            }
            Cartdetails.Add(cartdetails);
            cart.CartDetails = Cartdetails;


            var ResponseResult2 = await _cartService.AddToCartAsync<ResponseDto>(cart, accesstoken);
            if (ResponseResult2.IsSuccess == true && ResponseResult2 != null)
            {
                return RedirectToAction(nameof(Index));

            }
            return View(ProductDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
     [Authorize]
        public async Task<IActionResult> Login()
        {
         
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Logout()
        {
            return SignOut("Cookies","oidc");
        }
    }
}
