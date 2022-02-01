using Microsoft.AspNetCore.Mvc;
using Pet.Services.CouponAPI.Models.Dto;
using Pet.Services.CouponAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Services.CouponAPI.Controllers
{
    [ApiController]

    public class CouponAPIController : Controller
    {
        private ResponseDto _response;
        private ICouponRepository _repository;


        public CouponAPIController(ICouponRepository repository)
        {
            _repository = repository;
            _response = new ResponseDto();
        }
        [Route("api/Coupon/{couponCode}")]
        [HttpGet]
        public async Task<object> GetDiscountFromCouponCode(string couponCode)
        {
            try
            {
                var Result = await _repository.GetCouponCode(couponCode);
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

       
    }
}
