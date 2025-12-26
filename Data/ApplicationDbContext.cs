using Microsoft.EntityFrameworkCore;
using CoreBuilder.Entities;
using CoreBuilder.Services;

namespace CoreBuilder.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ITenantService _tenantService;
        private readonly int? _currentTenantId;

        // Constructor: Servisi içeri alıyoruz
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantService tenantService)
            : base(options)
        {
            _tenantService = tenantService;         
            // O anki site ID'sini servisten öğreniyoruz
            _currentTenantId = _tenantService.GetCurrentTenantId();
        }

        // Tablolarımız
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<ThemeSettings> ThemeSettings { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // --- GLOBAL QUERY FILTER (SİHİRLİ FİLTRE) ---
            // Her sorguya otomatik olarak "Where TenantId = X" ekler.
            // Böylece bir sitenin verisi diğerine asla karışmaz.
            builder.Entity<Page>().HasQueryFilter(p => p.TenantId == _currentTenantId);
            builder.Entity<ThemeSettings>().HasQueryFilter(t => t.TenantId == _currentTenantId);
            builder.Entity<ContactInfo>().HasQueryFilter(c => c.TenantId == _currentTenantId);

            // --- İLİŞKİ AYARLARI ---

            // Tenant silinirse ayarları da silinsin (Cascade Delete)
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
                .WithOne(p => p.Tenant) // explicit dependent navigation to avoid duplicate FK (TenantId1)
                .HasForeignKey(p => p.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}