using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pet.Services.ShoppingCartAPI.DbContexts;
using Pet.Services.ShoppingCartAPI.Models;
using Pet.Services.ShoppingCartAPI.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Services.ShoppingCartAPI.Repository
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private ApplicationDbContext _dbcontext { get; set; }
     
        private readonly IMapper _mapper;

        public ShoppingCartRepository(ApplicationDbContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
          
             _mapper=mapper;

    }
        public async  Task<bool> ClearCart(string UserId)
        {
            var resultfromdb = await _dbcontext.DbCartHeader.FirstOrDefaultAsync(u => u.UserId == UserId);
            if (resultfromdb != null)
            {
                _dbcontext.DbCartDetails.RemoveRange(_dbcontext.DbCartDetails.Where(u => u.CartHeaderId == resultfromdb.CartHeaderId));
                _dbcontext.DbCartHeader.RemoveRange(resultfromdb);
                await _dbcontext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async  Task<CartDto> CreateorUpdate(CartDto CartDto)
        {var f= _mapper.Map<IEnumerable<CartDetails>>(CartDto.CartDetails);
            var cart = _mapper.Map<Cart>(CartDto);
            var ResultProductId = await _dbcontext.DbProducts.AsNoTracking().FirstOrDefaultAsync(u => u.ProductId == CartDto.CartDetails.FirstOrDefault().ProductId);
            if (ResultProductId == null)
            {
                _dbcontext.DbProducts.Add(cart.CartDetails.FirstOrDefault().Product);
               await  _dbcontext.SaveChangesAsync();

            }

            var ResultCartheaderfromdb = await  _dbcontext.DbCartHeader.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == CartDto.CartHeader.UserId);

            if (ResultCartheaderfromdb == null)
            {
                _dbcontext.DbCartHeader.Add(cart.CartHeader);
                await _dbcontext.SaveChangesAsync();
                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.CartHeaderId;
                cart.CartDetails.FirstOrDefault().Product = null;
                _dbcontext.DbCartDetails.Add(cart.CartDetails.FirstOrDefault());
                await _dbcontext.SaveChangesAsync();
            }
            else
            {
                var resultcartdetailsfromdbs= await _dbcontext.DbCartDetails.AsNoTracking().FirstOrDefaultAsync(u => u.ProductId == CartDto.CartDetails.FirstOrDefault().ProductId && u.CartHeaderId == ResultCartheaderfromdb.CartHeaderId );
                if (resultcartdetailsfromdbs != null)
                {
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().Count += resultcartdetailsfromdbs.Count;
                    cart.CartDetails.FirstOrDefault().CartDetailsId = resultcartdetailsfromdbs.CartDetailsId;
                    cart.CartDetails.FirstOrDefault().CartHeaderId = resultcartdetailsfromdbs.CartHeaderId;
                    _dbcontext.DbCartDetails.Update(cart.CartDetails.FirstOrDefault());
                    await _dbcontext.SaveChangesAsync();
                }
                else
                {
                    cart.CartDetails.FirstOrDefault().CartHeaderId = ResultCartheaderfromdb.CartHeaderId;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    _dbcontext.DbCartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await _dbcontext.SaveChangesAsync();
                }


            }
           

           return  _mapper.Map<CartDto>(cart); 

        }
        public async Task<CartDto> GetCartByUserId(string UserId)
        {
            var resultfromdb =  await _dbcontext.DbCartHeader.FirstOrDefaultAsync(u => u.UserId == UserId);

            CartDto cartDto = new CartDto
            {
                CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(_dbcontext.DbCartDetails.Where(u => u.CartHeaderId == resultfromdb.CartHeaderId).Include(u => u.Product)),
                CartHeader = _mapper.Map<CartHeaderDto>(_dbcontext.DbCartHeader.FirstOrDefault(u => u.CartHeaderId == resultfromdb.CartHeaderId))

                };

            return cartDto;
        }

        public async  Task<bool> Remove(int CartDetailsId)
        {
            try
            {
                var resultfromdb = await _dbcontext.DbCartDetails.FirstOrDefaultAsync(u => u.CartDetailsId == CartDetailsId);
                if (resultfromdb != null)
                {
                    int TotalcountofCartItems = _dbcontext.DbCartDetails.Where(u => u.CartHeaderId == resultfromdb.CartHeaderId).Count();
                    _dbcontext.DbCartDetails.Remove(resultfromdb);



                    if (TotalcountofCartItems == 1)
                    {
                        _dbcontext.DbCartHeader.Remove(_dbcontext.DbCartHeader.FirstOrDefault(u => u.CartHeaderId == resultfromdb.CartHeaderId));
                    }
                    await _dbcontext.SaveChangesAsync();
                    return true;
                }
            }
            catch( Exception e)
            {
                return false;
            }
            return false;
        }
        public async Task<bool> ApplyCoupon(string UserId, string CouponCode)
        {

            var ResultCartheaderfromdb = await _dbcontext.DbCartHeader.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == UserId);
            if (ResultCartheaderfromdb != null)
            {

                ResultCartheaderfromdb.CouponCode = CouponCode;
                _dbcontext.DbCartHeader.Update(ResultCartheaderfromdb);
                await _dbcontext.SaveChangesAsync();
                return true;

            }
            return false;
        }
        public async Task<bool> RemoveCoupon(string UserId)
        {
            var ResultCartheaderfromdb = await _dbcontext.DbCartHeader.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == UserId);
            if (ResultCartheaderfromdb != null)
            {

                ResultCartheaderfromdb.CouponCode = " ";
                _dbcontext.DbCartHeader.Update(ResultCartheaderfromdb);
                await _dbcontext.SaveChangesAsync();
                return true;

            }
            return false;

        }
    }
}
