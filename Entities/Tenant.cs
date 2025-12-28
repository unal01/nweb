using System.ComponentModel.DataAnnotations;

namespace CoreBuilder.Entities
{
    public class Tenant : BaseEntity
    {
        [Required]
        public string SiteName { get; set; } = string.Empty; // Boş değer atadık (Uyarı gider)

        [Required]
        public string Domain { get; set; } = string.Empty; // Boş değer atadık (Uyarı gider)

        public string? Category { get; set; } // '?' koyduk, boş geçilebilir olsun

        public bool IsActive { get; set; } = true;

        // --- İLİŞKİLER ---

        // Soru işareti (?) koyuyoruz ki siteyi ilk oluştururken bu bilgiler henüz yoksa hata vermesin.
        public ThemeSettings? ThemeSettings { get; set; }
        public ContactInfo? ContactInfo { get; set; }

        // Listeyi başlatıyoruz (= new List<Page>()). 
        // Böylece sayfalar henüz yokken 'null' hatası almazsın.
        public ICollection<Page> Pages { get; set; } = new List<Page>();
    }
}