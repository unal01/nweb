namespace CoreBuilder.Entities
{
    public class ContactInfo : BaseEntity
    {
        public int TenantId { get; set; }
        public string? Phone1 { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? MapEmbedCode { get; set; }
        public string? SocialMediaJson { get; set; }
    }
}