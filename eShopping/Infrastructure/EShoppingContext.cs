using eShopping.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopping.Infrastructure
{
    public class EShoppingContext : IdentityDbContext<AppUser>
    {
        public EShoppingContext(DbContextOptions<EShoppingContext> options) : base(options)
        {
        }

        public DbSet<Page> Pages { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }
    }
}
