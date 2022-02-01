using Microsoft.AspNetCore.Mvc;
using Pet.MessageBus;
using Pet.Services.ShoppingCartAPI.Messages;
using Pet.Services.ShoppingCartAPI.Models.Dto;
using Pet.Services.ShoppingCartAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Services.ShoppingCartAPI.Controllers
{   [ApiController]
  
    public class CartAPIController : Controller
    {
        private ResponseDto _response;
        private IShoppingCartRepository _repository;
        private IMessageBus _messageBus;
        private ICouponRepository _couponRepository;
        private readonly Irabbit _rabbitMqbusmessage;
        public CartAPIController(IShoppingCartRepository repository,IMessageBus messagebus,ICouponRepository couponrepository, Irabbit rabbit)
        {
            _repository = repository;
            _response = new ResponseDto();
            _messageBus = messagebus;
            _couponRepository = couponrepository;
            _rabbitMqbusmessage = rabbit;
        }
        [Route("api/CartApi/GetCart/{UserId}")]
        [HttpGet]
        public async Task<object> GetCart(string UserId)
        {
            try {
                var Result = await _repository.GetCartByUserId(UserId);
                _response.IsSuccess = true;
                _response.Result = Result;

            }
            catch (Exception e)
            {

                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.Message.ToString() };

            }
            return _response;
        }


        [Route("api/CartApi/CreateCart")]
        [HttpPost]
        public async Task<object>CreateCart([FromBody]  CartDto CartDto)
            {
            try
            {
                var result =await  _repository.CreateorUpdate(CartDto);
                _response.Result = result;
            }
            catch(Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.Message.ToString() };

            }
            return _response;

        }
        [Route("api/CartApi/UpdateCart")]
        [HttpPost]
        public async Task<object> UpdateCart([FromBody] CartDto CartDto)
        {
            try
            {
                var result = await _repository.CreateorUpdate(CartDto);
                _response.Result = result;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.Message.ToString() };

            }
            return _response;

        }
        [Route("api/CartApi/Delete")]
        [HttpPost]
        public async Task<object> Delete([FromBody] int cartDetailId)
        {
            try
            {
                var result = await _repository.Remove(cartDetailId);
                _response.Result = result;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.Message.ToString() };

            }
            return _response;

        }
        [Route("api/CartApi/deleteCartAll")]
        [HttpPost]
        public async Task<bool> DeleteCartAll([FromBody] string UserId)
        {
            try
            {
                var result = await _repository.ClearCart(UserId);

            }
            catch (Exception e)
            {
                _response.IsSuccess = false;


            }
            return _response.IsSuccess;

        }
        [Route("api/CartApi/applyCoupon")]
        [HttpPost]
        public async Task<object> applyCoupon([FromBody] CartDto cartdto)
        {
            try
            {
                var result = await _repository.ApplyCoupon(cartdto.CartHeader.UserId,cartdto.CartHeader.CouponCode);
                _response.Result = result;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.Message.ToString() };

            }
            return _response;

        }
        [Route("api/CartApi/RemoveCoupon")]
        [HttpPost]
        public async Task<object> RemoveCoupon([FromBody] string UserId)
        {
            try
            {
                var result = await _repository.RemoveCoupon(UserId);
                _response.Result = result;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.Message.ToString() };

            }
            return _response;

        }
        [Route("api/CartApi/Checkout")]
        [HttpPost]
        public async Task<object> Checkout(CheckoutHeaderDto checkoutheaderdto)
        {

            try
            {
                CartDto cartdto = await _repository.GetCartByUserId(checkoutheaderdto.UserId);
                if (cartdto == null)
                {
                    return BadRequest();
                }
                checkoutheaderdto.CartDetails = cartdto.CartDetails;
                if (!string.IsNullOrEmpty(checkoutheaderdto.CouponCode))
                {
                    var responseCoupon = await _couponRepository.getcoupon(checkoutheaderdto.CouponCode);
                    if (checkoutheaderdto.DiscountTotal != responseCoupon.DiscountAmount)
                    {

                        _response.IsSuccess = false;
                        _response.ErrorMessages = new List<string> { "coupon code has changed please confirm" };
                        _response.DisplayMessage = "please the coupon code changed please confirm";
                        return _response;
                    }
                }


                //checkoutheaderdto.CartTotalItems = cartdto.CartDetails.Count();
                //await _messageBus.PublicMessage(checkoutheaderdto, "checkoutmessagetopic");
                await _rabbitMqbusmessage.PublicMessage(checkoutheaderdto, "CHECHOUTQUEUE");
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.Message.ToString() };

            }
            return _response;

        }
    }
}
