using System;
using System.ComponentModel.DataAnnotations;

namespace CoreBuilder.Entities
{
    public class ContactMessage
    {
        public int Id { get; set; }
        public int TenantId { get; set; } // Hangi siteye geldi?

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(200)]
        public string Subject { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Message { get; set; }

        public DateTime SentAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false; // Okundu mu?
    }
}