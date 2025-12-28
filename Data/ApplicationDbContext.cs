using Microsoft.EntityFrameworkCore;
using CoreBuilder.Entities;

namespace CoreBuilder.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Context instance'ındaki tenant id; bileşen/servis tarafından atanacak
        public int? CurrentTenantId { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<ThemeSettings> ThemeSettings { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Global query filter: context instance'ındaki CurrentTenantId kullanılır.
            builder.Entity<Page>().HasQueryFilter(p =>
                !EF.Property<int?>(this, nameof(CurrentTenantId)).HasValue
                || p.TenantId == EF.Property<int?>(this, nameof(CurrentTenantId)).Value);

            builder.Entity<ThemeSettings>().HasQueryFilter(t =>
                !EF.Property<int?>(this, nameof(CurrentTenantId)).HasValue
                || t.TenantId == EF.Property<int?>(this, nameof(CurrentTenantId)).Value);

            builder.Entity<ContactInfo>().HasQueryFilter(c =>
                !EF.Property<int?>(this, nameof(CurrentTenantId)).HasValue
                || c.TenantId == EF.Property<int?>(this, nameof(CurrentTenantId)).Value);

            // --- İlişki ayarları ---
            builder.Entity<Tenant>()
                .HasOne(t => t.ThemeSettings)
                .WithOne()
                .HasForeignKey<ThemeSettings>(ts => ts.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Tenant>()
                .HasOne(t => t.ContactInfo)
                .WithOne()
                .HasForeignKey<ContactInfo>(ci => ci.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Tenant>()
                .HasMany(t => t.Pages)
                .WithOne(p => p.Tenant)
                .HasForeignKey(p => p.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
