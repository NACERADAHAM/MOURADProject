using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PetStore.Web.Models;
using PetStore.Web.Services;
using PetStore.Web.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Web.Controllers
{
    public class CartController : Controller
    {
        IProductService _productService;
        private readonly ILogger<CartController> _logger;
        ICartService _cartService;
        ICouponService _couponservice;

        public CartController(IProductService product, ILogger<CartController> logger, ICartService Cartservice,ICouponService couponservice)
        {

            _logger = logger;
            _productService = product;
            _cartService = Cartservice;
            _couponservice = couponservice;

        }
        public async Task<IActionResult> CartIndex()
        {

            return View(await LoadCartDtoBaseOnLoggedInUser());
        }
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {

            return View(await LoadCartDtoBaseOnLoggedInUser());
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(CartDto cartdto)
        {
            try
            {
                var accesstoken = await HttpContext.GetTokenAsync("access_token");

                var Response = await _cartService.checkout<ResponseDto>(cartdto.CartHeader, accesstoken);
                if (Response.IsSuccess == false)
                {
                    TempData["Error"] = Response.DisplayMessage;

                    return RedirectToAction(nameof(Checkout));
                }
                return RedirectToAction(nameof(Confirmation));
            }
            catch
            {
                return View(cartdto);
            }

        }
        [HttpGet]
        public async Task<IActionResult> Confirmation()
        {
            return View();
        }


        private async Task<CartDto> LoadCartDtoBaseOnLoggedInUser()
        {

            var UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accesstoken = await HttpContext.GetTokenAsync("access_token");

            var Response = await _cartService.GetCartByUserIdAsync<ResponseDto>(UserId, accesstoken);

            CartDto cartdto = new();
           
            if (Response.IsSuccess = true && Response != null)
            {
                cartdto = JsonConvert.DeserializeObject<CartDto>(Response.Result.ToString());
            }
            if (cartdto.CartHeader != null)
            {
                if(cartdto.CartHeader.CouponCode!= " " && !string.IsNullOrEmpty(cartdto.CartHeader.CouponCode))
                {
                    var Responsecoupon = await _couponservice.GetCoupon<ResponseDto>(cartdto.CartHeader.CouponCode, accesstoken);
                    if (Responsecoupon.IsSuccess = true && Responsecoupon != null && Responsecoupon.Result!=null)
                    {
                        var coupondto = JsonConvert.DeserializeObject<CouponDto>(Responsecoupon.Result.ToString());
                        cartdto.CartHeader.DiscountTotal = coupondto.DiscountAmount;
                    }
                }
                foreach (var details in cartdto.CartDetails)
                {
                    cartdto.CartHeader.OrderTotal += details.Product.Price * details.Count;
                }
                cartdto.CartHeader.OrderTotal = cartdto.CartHeader.OrderTotal -  cartdto.CartHeader.DiscountTotal;
            }
            return cartdto;
        }
        [HttpPost]
        public async Task<IActionResult> Remove(int cartDetailsId)

        {
            var UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accesstoken = await HttpContext.GetTokenAsync("access_token");
            var Response = await _cartService.RemoveFromCartAsync<ResponseDto>(cartDetailsId, accesstoken);
            if (Response.IsSuccess = true && Response != null)
            {
                return RedirectToAction(nameof(CartIndex));

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon()

        {
            var UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accesstoken = await HttpContext.GetTokenAsync("access_token");
            var Response = await _cartService.RemoveCoupon<ResponseDto>(UserId, accesstoken);
            if (Response.IsSuccess = true && Response != null)
            {
                return RedirectToAction(nameof(CartIndex));

            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartdto)

        {
            var UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accesstoken = await HttpContext.GetTokenAsync("access_token");
            var Response = await _cartService.ApplyCoupon<ResponseDto>(cartdto, accesstoken);
            if (Response.IsSuccess = true && Response != null)
            {
                return RedirectToAction(nameof(CartIndex));

            }
            return View();
        }
        
       
    }
}
