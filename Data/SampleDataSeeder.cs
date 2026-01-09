// Bu dosyayý Program.cs'e dahil et veya bir kez çalýþtýr
// Örnek içerikleri veritabanýna ekler

using System.Text.Json;
using CoreBuilder.Data;
using Microsoft.EntityFrameworkCore;

public static class SampleDataSeeder
{
    public static async Task SeedSampleContent(ApplicationDbContext db)
    {
        var pages = await db.Pages.IgnoreQueryFilters().ToListAsync();
        
        foreach (var page in pages)
        {
            var content = GetContentForPage(page.Slug);
            if (!string.IsNullOrEmpty(content))
            {
                page.ContentJson = content;
            }
        }
        
        await db.SaveChangesAsync();
        Console.WriteLine($">>> {pages.Count} sayfa güncellendi!");
    }

    private static string GetContentForPage(string slug)
    {
        return slug switch
        {
            "ana-sayfa" => JsonSerializer.Serialize(new
            {
                PageType = "Standard",
                HasSlider = true,
                Sliders = new[]
                {
                    new { ImageUrl = "/uploads/slider/slider1.svg", Title = "Bartýn Sýnav Ýleri Kurs", LinkUrl = "" },
                    new { ImageUrl = "/uploads/slider/slider2.svg", Title = "Uzman Kadro", LinkUrl = "/hakkimizda" },
                    new { ImageUrl = "/uploads/slider/slider3.svg", Title = "2024 Kayýtlarý", LinkUrl = "/iletisim" }
                },
                HasNews = true,
                NewsItems = new[]
                {
                    new { Title = "YKS'de Büyük Baþarý!", Summary = "Öðrencilerimiz 2024 YKS'de rekor kýrdý", ImageUrl = "/uploads/news/haber1.svg", LinkUrl = "", Content = "" },
                    new { Title = "Ödül Törenimiz", Summary = "Baþarýlý öðrencilerimize ödüllerini takdim ettik", ImageUrl = "/uploads/news/haber2.svg", LinkUrl = "", Content = "" }
                },
                HasAnnouncements = true,
                Announcements = new[]
                {
                    new { Title = "2024-2025 Kayýtlarý Baþladý", ImageUrl = "/uploads/announcements/duyuru1.svg", LinkUrl = "/iletisim", Content = "Erken kayýt avantajlarýndan yararlanýn!" },
                    new { Title = "Deneme Sýnavý Takvimi Açýklandý", ImageUrl = "/uploads/announcements/duyuru2.svg", LinkUrl = "/duyurular", Content = "Tüm sýnav tarihlerini inceleyin" }
                },
                HasGallery = false,
                GalleryItems = new object[] { },
                HtmlContent = @"<div class='text-center py-4'>
                    <h2 class='text-primary'>Hoþ Geldiniz!</h2>
                    <p class='lead'>Bartýn Sýnav Ýleri Kurs olarak 20 yýlý aþkýn tecrübemizle öðrencilerimizi hayallerine kavuþturuyoruz.</p>
                    <div class='row mt-4'>
                        <div class='col-md-4'><div class='p-3 bg-light rounded'><h3 class='text-success'>10.000+</h3><p>Mezun Öðrenci</p></div></div>
                        <div class='col-md-4'><div class='p-3 bg-light rounded'><h3 class='text-primary'>50+</h3><p>Uzman Öðretmen</p></div></div>
                        <div class='col-md-4'><div class='p-3 bg-light rounded'><h3 class='text-warning'>%95</h3><p>Baþarý Oraný</p></div></div>
                    </div>
                </div>",
                Contact = new { }
            }),

            "hakkimizda" => JsonSerializer.Serialize(new
            {
                PageType = "Standard",
                HasSlider = false,
                Sliders = new object[] { },
                HasNews = false,
                NewsItems = new object[] { },
                HasAnnouncements = false,
                Announcements = new object[] { },
                HasGallery = false,
                GalleryItems = new object[] { },
                HtmlContent = @"<div class='container'>
                    <h2>Hakkýmýzda</h2>
                    <p class='lead'>Bartýn Sýnav Ýleri Kurs, 2004 yýlýndan bu yana Bartýn'da eðitim hizmeti vermektedir.</p>
                    <hr>
                    <h4>Tarihçemiz</h4>
                    <p>Kurumumuz, küçük bir dershaneden baþlayarak bugün Bartýn'ýn en köklü eðitim kurumlarýndan biri haline gelmiþtir.</p>
                    <h4>Deðerlerimiz</h4>
                    <ul>
                        <li>Öðrenci odaklý eðitim anlayýþý</li>
                        <li>Sürekli geliþim ve yenilikçilik</li>
                        <li>Akademik mükemmellik</li>
                        <li>Etik ve þeffaflýk</li>
                    </ul>
                </div>",
                Contact = new { }
            }),

            "yonetim" => JsonSerializer.Serialize(new
            {
                PageType = "Standard",
                HasSlider = false,
                Sliders = new object[] { },
                HasNews = false,
                NewsItems = new object[] { },
                HasAnnouncements = false,
                Announcements = new object[] { },
                HasGallery = false,
                GalleryItems = new object[] { },
                HtmlContent = @"<div class='container'>
                    <h2>Yönetim Kadromuz</h2>
                    <div class='row mt-4'>
                        <div class='col-md-4 text-center mb-4'>
                            <div class='p-4 bg-light rounded'>
                                <div class='bg-primary text-white rounded-circle d-inline-flex align-items-center justify-content-center' style='width:80px;height:80px;font-size:2rem'>?????</div>
                                <h5 class='mt-3'>Ahmet Yýlmaz</h5>
                                <p class='text-muted'>Kurucu & Genel Müdür</p>
                            </div>
                        </div>
                        <div class='col-md-4 text-center mb-4'>
                            <div class='p-4 bg-light rounded'>
                                <div class='bg-success text-white rounded-circle d-inline-flex align-items-center justify-content-center' style='width:80px;height:80px;font-size:2rem'>?????</div>
                                <h5 class='mt-3'>Fatma Demir</h5>
                                <p class='text-muted'>Eðitim Koordinatörü</p>
                            </div>
                        </div>
                        <div class='col-md-4 text-center mb-4'>
                            <div class='p-4 bg-light rounded'>
                                <div class='bg-info text-white rounded-circle d-inline-flex align-items-center justify-content-center' style='width:80px;height:80px;font-size:2rem'>?????</div>
                                <h5 class='mt-3'>Mehmet Kaya</h5>
                                <p class='text-muted'>Akademik Danýþman</p>
                            </div>
                        </div>
                    </div>
                </div>",
                Contact = new { }
            }),

            "misyonvizyon" => JsonSerializer.Serialize(new
            {
                PageType = "Standard",
                HasSlider = false,
                Sliders = new object[] { },
                HasNews = false,
                NewsItems = new object[] { },
                HasAnnouncements = false,
                Announcements = new object[] { },
                HasGallery = false,
                GalleryItems = new object[] { },
                HtmlContent = @"<div class='container'>
                    <div class='row'>
                        <div class='col-md-6 mb-4'>
                            <div class='card h-100 border-primary'>
                                <div class='card-header bg-primary text-white'><h4 class='mb-0'>?? Misyonumuz</h4></div>
                                <div class='card-body'>
                                    <p>Öðrencilerimize kaliteli eðitim vererek onlarý hayallerine kavuþturmak, akademik baþarýlarýný en üst düzeye çýkarmak ve topluma faydalý bireyler yetiþtirmektir.</p>
                                </div>
                            </div>
                        </div>
                        <div class='col-md-6 mb-4'>
                            <div class='card h-100 border-success'>
                                <div class='card-header bg-success text-white'><h4 class='mb-0'>?? Vizyonumuz</h4></div>
                                <div class='card-body'>
                                    <p>Türkiye'nin en baþarýlý eðitim kurumlarý arasýnda yer almak, yenilikçi eðitim metodlarýyla öncü olmak ve her öðrencinin potansiyelini maksimum düzeyde ortaya çýkarmaktýr.</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>",
                Contact = new { }
            }),

            "duyurular" => JsonSerializer.Serialize(new
            {
                PageType = "Standard",
                HasSlider = false,
                Sliders = new object[] { },
                HasNews = false,
                NewsItems = new object[] { },
                HasAnnouncements = true,
                Announcements = new[]
                {
                    new { Title = "2024-2025 Kayýtlarý Baþladý", ImageUrl = "/uploads/announcements/duyuru1.svg", LinkUrl = "/iletisim", Content = "Yeni eðitim dönemi kayýtlarýmýz baþlamýþtýr. Erken kayýt yaptýran öðrencilerimize %20 indirim uygulanacaktýr." },
                    new { Title = "Deneme Sýnavý Takvimi", ImageUrl = "/uploads/announcements/duyuru2.svg", LinkUrl = "", Content = "Kasým ayý deneme sýnavýmýz 15 Kasým Cumartesi günü yapýlacaktýr. Tüm öðrencilerimizin katýlýmýný bekliyoruz." },
                    new { Title = "Veli Toplantýsý", ImageUrl = "", LinkUrl = "", Content = "20 Kasým Çarþamba günü saat 19:00'da veli toplantýmýz yapýlacaktýr." }
                },
                HasGallery = false,
                GalleryItems = new object[] { },
                HtmlContent = @"<div class='container'><h2>?? Duyurular</h2><p class='text-muted'>Kurumumuza ait güncel duyurular aþaðýda listelenmiþtir.</p><hr></div>",
                Contact = new { }
            }),

            "rehberlik" => JsonSerializer.Serialize(new
            {
                PageType = "Standard",
                HasSlider = false,
                Sliders = new object[] { },
                HasNews = false,
                NewsItems = new object[] { },
                HasAnnouncements = false,
                Announcements = new object[] { },
                HasGallery = false,
                GalleryItems = new object[] { },
                HtmlContent = @"<div class='container'>
                    <h2>?? Rehberlik Hizmetlerimiz</h2>
                    <p class='lead'>Uzman rehberlik kadromuz öðrencilerimize her aþamada destek olmaktadýr.</p>
                    <div class='row mt-4'>
                        <div class='col-md-6 mb-3'>
                            <div class='card border-info'>
                                <div class='card-body'>
                                    <h5>?? Akademik Rehberlik</h5>
                                    <p>Ders çalýþma teknikleri, zaman yönetimi ve verimli öðrenme stratejileri konusunda destek.</p>
                                </div>
                            </div>
                        </div>
                        <div class='col-md-6 mb-3'>
                            <div class='card border-warning'>
                                <div class='card-body'>
                                    <h5>?? Kariyer Danýþmanlýðý</h5>
                                    <p>Tercih döneminde bölüm ve üniversite seçimi konusunda profesyonel rehberlik.</p>
                                </div>
                            </div>
                        </div>
                        <div class='col-md-6 mb-3'>
                            <div class='card border-success'>
                                <div class='card-body'>
                                    <h5>?? Motivasyon Desteði</h5>
                                    <p>Sýnav kaygýsý yönetimi ve motivasyon artýrýcý bireysel görüþmeler.</p>
                                </div>
                            </div>
                        </div>
                        <div class='col-md-6 mb-3'>
                            <div class='card border-danger'>
                                <div class='card-body'>
                                    <h5>???????? Veli Görüþmeleri</h5>
                                    <p>Düzenli veli bilgilendirme toplantýlarý ve bireysel görüþmeler.</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>",
                Contact = new { }
            }),

            "hizmetlerimiz" => JsonSerializer.Serialize(new
            {
                PageType = "Standard",
                HasSlider = false,
                Sliders = new object[] { },
                HasNews = false,
                NewsItems = new object[] { },
                HasAnnouncements = false,
                Announcements = new object[] { },
                HasGallery = false,
                GalleryItems = new object[] { },
                HtmlContent = @"<div class='container'>
                    <h2>?? Eðitim Programlarýmýz</h2>
                    <div class='row mt-4'>
                        <div class='col-md-6 col-lg-3 mb-4'>
                            <div class='card h-100 text-center border-primary'>
                                <div class='card-body'>
                                    <div class='display-4 text-primary'>??</div>
                                    <h5>YKS Hazýrlýk</h5>
                                    <p class='small text-muted'>TYT ve AYT kurslarý</p>
                                    <span class='badge bg-primary'>En Popüler</span>
                                </div>
                            </div>
                        </div>
                        <div class='col-md-6 col-lg-3 mb-4'>
                            <div class='card h-100 text-center border-success'>
                                <div class='card-body'>
                                    <div class='display-4 text-success'>??</div>
                                    <h5>LGS Hazýrlýk</h5>
                                    <p class='small text-muted'>8. sýnýf öðrencileri için</p>
                                </div>
                            </div>
                        </div>
                        <div class='col-md-6 col-lg-3 mb-4'>
                            <div class='card h-100 text-center border-warning'>
                                <div class='card-body'>
                                    <div class='display-4 text-warning'>??</div>
                                    <h5>KPSS Hazýrlýk</h5>
                                    <p class='small text-muted'>Memur adaylarý için</p>
                                </div>
                            </div>
                        </div>
                        <div class='col-md-6 col-lg-3 mb-4'>
                            <div class='card h-100 text-center border-info'>
                                <div class='card-body'>
                                    <div class='display-4 text-info'>??</div>
                                    <h5>DGS Hazýrlýk</h5>
                                    <p class='small text-muted'>Dikey geçiþ sýnavý</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>",
                Contact = new { }
            }),

            "iletisim" => JsonSerializer.Serialize(new
            {
                PageType = "Contact",
                HasSlider = false,
                Sliders = new object[] { },
                HasNews = false,
                NewsItems = new object[] { },
                HasAnnouncements = false,
                Announcements = new object[] { },
                HasGallery = false,
                GalleryItems = new object[] { },
                HtmlContent = "",
                Contact = new
                {
                    PhoneLabel1 = "Santral",
                    Phone1 = "0378 227 00 00",
                    PhoneLabel2 = "WhatsApp",
                    Phone2 = "0532 000 00 00",
                    Email1 = "info@bartinsinavkurs.com",
                    Email2 = "kayit@bartinsinavkurs.com",
                    AddressText = "Kemerköprü Mah. Cumhuriyet Cad. No:123",
                    City = "Bartýn",
                    District = "Merkez",
                    MapEmbedCode = "<iframe src='https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d47982.79236567093!2d32.31!3d41.63!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x0!2zNDHCsDM4JzAwLjAiTiAzMsKwMTgnMDAuMCJF!5e0!3m2!1str!2str!4v1234567890' width='100%' height='300' style='border:0;' allowfullscreen='' loading='lazy'></iframe>",
                    WorkingDays = "Pazartesi - Cumartesi",
                    WorkingHours = "08:30 - 21:00",
                    Whatsapp = "05320000000",
                    Instagram = "bartinsinavkurs",
                    Facebook = "bartinsinavkurs",
                    Twitter = "bartinsinavkurs"
                }
            }),

            "galeri" => JsonSerializer.Serialize(new
            {
                PageType = "Standard",
                HasSlider = false,
                Sliders = new object[] { },
                HasNews = false,
                NewsItems = new object[] { },
                HasAnnouncements = false,
                Announcements = new object[] { },
                HasGallery = true,
                GalleryItems = new[]
                {
                    new { ImageUrl = "/uploads/gallery/galeri1.svg", Title = "Kurs Binamýz", Description = "Modern ve ferah eðitim ortamýmýz" },
                    new { ImageUrl = "/uploads/gallery/galeri2.svg", Title = "Dersliklerimiz", Description = "Akýllý tahta donanýmlý sýnýflar" },
                    new { ImageUrl = "/uploads/gallery/galeri3.svg", Title = "Kütüphanemiz", Description = "Zengin kaynak arþivi" },
                    new { ImageUrl = "/uploads/gallery/galeri4.svg", Title = "Bilgisayar Lab.", Description = "Online sýnav ve araþtýrma merkezi" },
                    new { ImageUrl = "/uploads/gallery/galeri5.svg", Title = "Mezuniyet", Description = "2024 mezuniyet törenimiz" },
                    new { ImageUrl = "/uploads/gallery/galeri6.svg", Title = "Baþarýlarýmýz", Description = "Ödül alan öðrencilerimiz" }
                },
                HtmlContent = @"<div class='container'><h2>?? Fotoðraf Galerisi</h2><p class='text-muted'>Kurumumuzdan kareler...</p><hr></div>",
                Contact = new { }
            }),

            _ => null
        };
    }
}
