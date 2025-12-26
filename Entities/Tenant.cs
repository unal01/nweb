using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Pkcs;

namespace CoreBuilder.Entities
{
    public class Tenant : BaseEntity
    {
        [Required]
        public string SiteName { get; set; } // Örn: Bartin Sinav Ileri Kurs

        [Required]
        public string Domain { get; set; } // Örn: sinav.localhost

        public string Category { get; set; } // Egitim, Kurumsal vb.

        public bool IsActive { get; set; } = true;

        // İlişkiler
        public ThemeSettings ThemeSettings { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public ICollection<Page> Pages { get; set; }
    }
}