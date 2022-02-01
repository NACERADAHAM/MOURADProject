using Microsoft.EntityFrameworkCore;
using Pet.Services.OrderAPI.DbContexts;
using Pet.Services.OrderAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Services.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbcontext;
        public OrderRepository(DbContextOptions<ApplicationDbContext> dbcontext) {
            _dbcontext = dbcontext;
        }
        public  async Task<bool> AddOrder(OrderHeader orderHeader)
        {
            await using var _db = new ApplicationDbContext(_dbcontext);
            var result = _db.DbOrderHeader.Add(orderHeader);
            await _db.SaveChangesAsync();
            return true;
        }

        public async  Task UpdateOrderPayementStatus(int orderHeaderId, bool paid)
        {
            await using var _db = new ApplicationDbContext(_dbcontext);
            var result = await _db.DbOrderHeader.FirstOrDefaultAsync(u => u.OrderHeaderId == orderHeaderId);
            if (result != null)
            {
                result.paymentstatus = paid;
                _db.DbOrderHeader.Update(result);
                await _db.SaveChangesAsync();
            }
        
        }
    }
}
