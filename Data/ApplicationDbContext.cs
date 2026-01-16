using CoreBuilder.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreBuilder.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Context instance'ýndaki tenant id; bileþen/servis tarafýndan atanacak
        public int? CurrentTenantId { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<StudentRegistration> StudentRegistrations { get; set; }
        public DbSet<ThemeSettings> ThemeSettings { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }

        // YENÝ EKLENEN: Ýletiþim Mesajlarý Tablosu
        public DbSet<ContactMessage> ContactMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // --- GLOBAL QUERY FILTERS (FÝLTRELER) ---
            // Bu filtreler sayesinde bir tenant (site) seçiliyken, yanlýþlýkla baþka sitenin verisi gelmez.

            builder.Entity<Page>().HasQueryFilter(p =>
                !CurrentTenantId.HasValue || p.TenantId == CurrentTenantId);

            builder.Entity<ThemeSettings>().HasQueryFilter(t =>
                !CurrentTenantId.HasValue || t.TenantId == CurrentTenantId);

            builder.Entity<ContactInfo>().HasQueryFilter(c =>
                !CurrentTenantId.HasValue || c.TenantId == CurrentTenantId);

            builder.Entity<StudentRegistration>().HasQueryFilter(s =>
                !CurrentTenantId.HasValue || s.TenantId == CurrentTenantId);

            // YENÝ EKLENEN: Mesajlar için güvenlik filtresi
            builder.Entity<ContactMessage>().HasQueryFilter(m =>
                !CurrentTenantId.HasValue || m.TenantId == CurrentTenantId);


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
                .HasOne<Tenant>()
                .WithMany()
                .HasForeignKey(s => s.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            // YENÝ EKLENEN: Mesaj silme ayarý (Site silinirse mesajlarý da silinsin)
            builder.Entity<ContactMessage>()
                .HasOne<Tenant>()
                .WithMany()
                .HasForeignKey(m => m.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}