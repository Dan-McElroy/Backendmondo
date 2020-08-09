using Backendmondo.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backendmondo.API.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Product> Products { get; private set; }

        public DbSet<Subscription> Subscriptions { get; private set; }

        public DbSet<SubscriptionPause> SubscriptionPauses { get; private set; }

        public DbSet<User> Users { get; private set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public async Task Save() => await base.SaveChangesAsync();
    }
}
