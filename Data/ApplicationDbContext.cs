using CoreBuilder.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreBuilder.Data
{
    public class ApplicationDbContext : DbContext
    {
        public int? CurrentTenantId { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }
        // Eklediðin satýr doðru:
        public DbSet<StudentRegistration> StudentRegistrations { get; set; }
        public DbSet<ThemeSettings> ThemeSettings { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // --- GLOBAL QUERY FILTERS (FÝLTRELER) ---

            builder.Entity<Page>().HasQueryFilter(p =>
                CurrentTenantId == null || p.TenantId == CurrentTenantId.Value);

            builder.Entity<ThemeSettings>().HasQueryFilter(t =>
                !CurrentTenantId.HasValue || t.TenantId == CurrentTenantId);

            builder.Entity<ContactInfo>().HasQueryFilter(c =>
                CurrentTenantId == null || c.TenantId == CurrentTenantId.Value);

            // !!! BU KISMI EKLEMEN ÇOK ÖNEMLÝ !!!
            // Bu satýr sayesinde her site sadece kendi öðrencisini görür.
            builder.Entity<StudentRegistration>().HasQueryFilter(s =>
                CurrentTenantId == null || s.TenantId == CurrentTenantId.Value);


            // --- ÝLÝÞKÝ AYARLARI ---

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

            // Öðrenci kaydý silme ayarý (Site silinirse öðrencileri de silinsin)
            builder.Entity<StudentRegistration>()
                .HasOne<Tenant>() // Tenant ile iliþkisi var
                .WithMany()       // Tenant tarafýnda bir liste tutulmuyor olabilir
                .HasForeignKey(s => s.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}