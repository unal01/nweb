using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreBuilder.Entities
{
    public class Page
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        public string Title { get; set; } = "";

        public string Slug { get; set; } = "";

        public string? ContentJson { get; set; } // İçerik (HTML)

        public bool IsPublished { get; set; } = true;
        public int Order { get; set; } = 0;

        // --- EKLENEN YENİ ÖZELLİKLER (Hataları çözecek kısım) ---
        public int? ParentId { get; set; } // Üst menü ID'si (Boş olabilir)

        public string PageType { get; set; } = "Standard"; // Standard, Gallery, Contact

        // Modül Ayarları (Kutucuklar)
        public bool HasSlider { get; set; } = false;
        public bool HasNews { get; set; } = false;
        public bool HasAnnouncements { get; set; } = false;
        public bool HasGallery { get; set; } = false;

        // İlişkiler
        public int TenantId { get; set; }
        public Tenant? Tenant { get; set; }
    }
}