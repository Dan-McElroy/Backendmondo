using Backendmondo.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backendmondo.API.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private const string IsDeletedKey = "IsDeleted";

        public DbSet<Product> Products { get; private set; }

        public DbSet<Subscription> Subscriptions { get; private set; }

        public DbSet<SubscriptionPause> SubscriptionPauses { get; private set; }

        public DbSet<User> Users { get; private set; }

        public DbSet<ProductPurchase> ProductPurchases { get; private set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public async Task Save() => await SaveChangesAsync();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Subscription>()
                .Property<bool>(IsDeletedKey);

            modelBuilder.Entity<Subscription>()
                .HasQueryFilter(entity => EF.Property<bool>(entity, IsDeletedKey) == false);
        }

        public override int SaveChanges()
        {
            UpdateSoftDeleteEntities();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateSoftDeleteEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateSoftDeleteEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateSoftDeleteEntities()
        {
            ChangeTracker.DetectChanges();

            var softDeletables = ChangeTracker.Entries().Where(entry => entry.Entity is ISoftDelete);

            foreach (var entry in softDeletables)
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        entry.CurrentValues[IsDeletedKey] = true;
                        break;
                    case EntityState.Added:
                        entry.CurrentValues[IsDeletedKey] = false;
                        break;
                }
            }
        }
    }
}
