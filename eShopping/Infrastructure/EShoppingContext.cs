using eShopping.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopping.Infrastructure
{
    public class EShoppingContext : DbContext
    {
        public EShoppingContext(DbContextOptions<EShoppingContext> options) : base(options)
        {
        }

        public DbSet<Page> Pages { get; set; }
    }
}
