using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient; // SQL bağlantısı için

namespace CoreBuilder.Services
{
    public class TenantService : ITenantService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _connectionString;

        public TenantService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            // Veritabanı adresini (Connection String) alıyoruz
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public string GetCurrentDomain()
        {
            // Tarayıcıdaki adresi al (Örn: sinav.localhost veya localhost:5000)
            return _httpContextAccessor.HttpContext?.Request.Host.Value ?? "localhost";
        }

        public int? GetCurrentTenantId()
        {
            var host = GetCurrentDomain();

            // KISIR DÖNGÜ ÖNLEMİ:
            // ApplicationDbContext kullanırsak döngüye girer.
            // O yüzden burada doğrudan SQL sorgusu atıyoruz. Bu çok hızlı ve güvenlidir.

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    // Domain veritabanında var mı diye bakıyoruz
                    var query = "SELECT Id FROM Tenants WHERE Domain = @Domain";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Domain", host);
                        var result = cmd.ExecuteScalar(); // İlk satırı getir

                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch
            {
                // Veritabanı bağlantısında sorun olursa null dön
                return null;
            }

            return null; // Bulunamazsa null dön
        }
    }
}