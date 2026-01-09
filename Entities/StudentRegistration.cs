using System;
using System.ComponentModel.DataAnnotations;

namespace CoreBuilder.Entities
{
    public class StudentRegistration
    {
        public int Id { get; set; }
        public int TenantId { get; set; } // Hangi şubeye/siteye ait olduğu

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(50)]
        public string Grade { get; set; } // Sınıf

        [MaxLength(200)]
        public string SchoolName { get; set; }

        [MaxLength(1000)]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsProcessed { get; set; } = false; // Okundu/Arandı mı?
    }
}