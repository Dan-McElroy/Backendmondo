using Backendmondo.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Backendmondo.API.Context
{
    public interface IApplicationDbContext
    {
        DbSet<Product> Products { get; }
        DbSet<SubscriptionPause> SubscriptionPauses { get; }
        DbSet<Subscription> Subscriptions { get; }
        DbSet<User> Users { get; }
        DbSet<ProductPurchase> ProductPurchases { get; }

        Task Save();
    }
}