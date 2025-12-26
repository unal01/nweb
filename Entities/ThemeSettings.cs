using System.ComponentModel.DataAnnotations;

namespace CoreBuilder.Entities
{
    public class ThemeSettings : BaseEntity
    {
        public int TenantId { get; set; }

        // Renkler
        public string PrimaryColor { get; set; } = "#0d6efd";
        public string SecondaryColor { get; set; } = "#6c757d";
        public string HeaderBgColor { get; set; } = "#ffffff";
        // ... Diğer özellikler ...

        public string? LogoUrl { get; set; } // Logo resminin adresi
        public string MenuTextColor { get; set; } = "#333333"; // Menü linklerinin rengi

        // ...

        // Fontlar
        public string FontFamily { get; set; } = "Inter";
        public string MenuType { get; set; } = "Yatay"; // Yatay, Dikey

        // Tipografi
        public int BaseFontSize { get; set; } = 14;

        // YENİ EKLENECEK ALANLAR:
        public bool ShowLogo { get; set; } = true;      // Logoyu gösterelim mi?
        public bool ShowSiteName { get; set; } = true;  // Site adını gösterelim mi?
    }
}