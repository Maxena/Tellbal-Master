using Common.Utilities;
using Entities;
using Entities.Identity;
using Entities.Product;
using Entities.Product.Customers;
using Entities.Product.Customers.DynamicPricing;
using Entities.Product.Dynamic;
using Entities.System;
using Entities.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderDetail> OrderDetails { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<PropertyKey> PropertyKeys { get; set; }

        public virtual DbSet<PropertyValue> PropertyValues { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Like> Likes { get; set; }

        public virtual DbSet<Province> Provinces { get; set; }

        public virtual DbSet<City> Cities { get; set; }

        public virtual DbSet<Address> Addresses { get; set; }

        public virtual DbSet<Image> Images { get; set; }

        public virtual DbSet<FastPricingKey> FastPricingKeys { get; set; }

        public virtual DbSet<FastPricingDD> FastPricingDDs { get; set; }

        public virtual DbSet<FastPricingValue> FastPricingValues { get; set; }

        public virtual DbSet<PriceLog> PriceLogs { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<FastPricingDefinition> FastPricingDefinitions { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<SellRequest> SellRequests { get; set; }
        public virtual DbSet<FAQ> FAQs { get; set; }
        public virtual DbSet<AppVariable> AppVariables { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasAnnotation(
                "Relational:Collation",
                "Persian_100_CI_AS");

            var entitiesAssembly = typeof(IEntity).Assembly;

            modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
            modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);
            modelBuilder.AddRestrictDeleteBehaviorConvention();
            modelBuilder.AddSequentialGuidForIdConvention();
            modelBuilder.AddPluralizingTableNameConvention();

            modelBuilder.Entity<User>(x => x.Property(p => p.Id).HasDefaultValueSql("NEWID()"));

            modelBuilder.Entity<Role>(x => x.Property(p => p.Id).HasDefaultValueSql("NEWID()"));
        }
        public override int SaveChanges()
        {
            _cleanString();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            _cleanString();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            _cleanString();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _cleanString();
            return base.SaveChangesAsync(cancellationToken);
        }
        private void _cleanString()
        {
            var changedEntities = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            foreach (var item in changedEntities)
            {
                if (item.Entity == null)
                    continue;

                var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

                foreach (var property in properties)
                {
                    var propName = property.Name;
                    var val = (string)property.GetValue(item.Entity, null);

                    if (val.HasValue())
                    {
                        var newVal = val.Fa2En().FixPersianChars();
                        if (newVal == val)
                            continue;
                        property.SetValue(item.Entity, newVal, null);
                    }
                }
            }
        }
    }
}
