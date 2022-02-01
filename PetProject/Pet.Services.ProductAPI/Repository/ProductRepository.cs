using Pet.Services.ProductAPI.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pet.Services.ProductAPI.DbContexts;
using AutoMapper;
using Pet.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Pet.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public ProductRepository(ApplicationDbContext DbContext,IMapper mapper)
        {
            _dbContext = DbContext;
            _mapper = mapper;
        }
        public async Task<ProductDto> CreateUpdateProduct(ProductDto productDto)
        {
           var Product = _mapper.Map<Product>(productDto);
           var ProductId = Product.ProductId;
            if (ProductId > 0)
            {
                _dbContext.Update(Product);
            }
            else
            {
                _dbContext.Add(Product);                
            }
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<ProductDto>(Product);
        }

        public async  Task<bool> DeleteProduct(int ProductId)
        {
            try
            {
                var ResultProductFromDb = await _dbContext.DbProducts.Where(x => x.ProductId == ProductId).FirstOrDefaultAsync();
                _dbContext.DbProducts.Remove(ResultProductFromDb);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<ProductDto> GetProductById(int ProductId)
        {
            var ResultProductFromDb = await _dbContext.DbProducts.Where(x=>x.ProductId==ProductId).FirstOrDefaultAsync();
            return _mapper.Map<ProductDto>(ResultProductFromDb);
        }

        public async Task<IEnumerable<ProductDto>> Getproducts()
        {
            var ResultProductsFromDb = await  _dbContext.DbProducts.ToListAsync();
            return _mapper.Map<List<ProductDto>>(ResultProductsFromDb);

        }
    }
}
