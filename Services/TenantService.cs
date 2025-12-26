using Microsoft.AspNetCore.Http;

namespace CoreBuilder.Services
{
    public class TenantService : ITenantService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentDomain()
        {
            // O anki tarayıcıdaki adresi (host) alır. Örn: sinav.localhost
            return _httpContextAccessor.HttpContext?.Request.Host.Value ?? "localhost";
        }

        public int? GetCurrentTenantId()
        {
            // MIGRATION İÇİN GEÇİCİ AYAR:
            // Veritabanı henüz oluşmadığı için şimdilik sabit "1" dönüyoruz.
            // Sistem çalışmaya başladığında burayı gerçek mantıkla değiştireceğiz.
            return 1;
        }
    }
}