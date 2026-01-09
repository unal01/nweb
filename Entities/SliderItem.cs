using System.ComponentModel.DataAnnotations;

namespace CoreBuilder.Entities
{
    public class SliderItem : BaseEntity
    {
        public int TenantId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = "";

        public string? Description { get; set; }

        [Required]
        public string ImageUrl { get; set; } = "";

        public string? LinkUrl { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation
        public Tenant? Tenant { get; set; }
    }
}