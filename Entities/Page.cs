using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreBuilder.Entities
{
    public class Page : BaseEntity
    {
        public int TenantId { get; set; } // Bu sütun veritabanında var

        [Required(ErrorMessage = "Sayfa başlığı zorunludur.")]
        public string Title { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public int Order { get; set; }

        public bool IsPublished { get; set; } = true;

        public string? ContentJson { get; set; }

        // --- İŞTE ÇÖZÜM BURASI ---
        // Bu [ForeignKey] satırı olmazsa, program 'TenantId1' diye hata verir!
        [ForeignKey("TenantId")]
        public Tenant? Tenant { get; set; }
    }
}