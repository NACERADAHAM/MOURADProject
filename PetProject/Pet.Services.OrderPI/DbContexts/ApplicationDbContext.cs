using Microsoft.EntityFrameworkCore;
using Pet.Services.OrderAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Services.OrderAPI.DbContexts
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {

        }
        public DbSet<OrderHeader> DbOrderHeader { get; set; }
        public DbSet<OrderDetails> DbOrderDetails { get; set; }
      
      



    }
}
