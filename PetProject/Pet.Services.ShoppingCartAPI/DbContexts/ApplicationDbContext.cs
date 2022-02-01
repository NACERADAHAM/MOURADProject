using Microsoft.EntityFrameworkCore;
using Pet.Services.ShoppingCartAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Services.ShoppingCartAPI.DbContexts
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {

        }
        public DbSet<Product> DbProducts { get; set; }
        public DbSet<CartHeader> DbCartHeader { get; set; }
        public DbSet<CartDetails> DbCartDetails { get; set; }
      



    }
}
