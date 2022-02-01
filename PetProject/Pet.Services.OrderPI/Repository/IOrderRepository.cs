using Pet.Services.OrderAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Services.OrderAPI.Repository
{
    public interface IOrderRepository
    {
        Task UpdateOrderPayementStatus(int orderHeaderId, bool paid);
        Task<bool> AddOrder(OrderHeader orderHeader);
    }
}
