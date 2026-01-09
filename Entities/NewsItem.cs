using System.ComponentModel.DataAnnotations;

namespace CoreBuilder.Entities
{
    public class NewsItem : BaseEntity
    {
        public int TenantId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = "";

        public string? Summary { get; set; }

        public string? Content { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime PublishDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        // Navigation
        public Tenant? Tenant { get; set; }
    }
}