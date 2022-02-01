using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pet.Services.CouponAPI.DbContexts;
using Pet.Services.CouponAPI.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Services.CouponAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _DbCoupon;
        protected IMapper _mapper;
        public CouponRepository(ApplicationDbContext dbcontext, IMapper MAPPER)
        {

            _DbCoupon = dbcontext;
            _mapper = MAPPER;
        }
        public async Task<CouponDto> GetCouponCode(string couponCode)
        {
            var ResultGetCouponFromDb = await  _DbCoupon.DbCoupons.FirstOrDefaultAsync(u => u.CouponCode == couponCode);
                return _mapper.Map<CouponDto>(ResultGetCouponFromDb);
        }
    }
}
