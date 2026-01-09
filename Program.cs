using CoreBuilder.Components;
using CoreBuilder.Data;
using CoreBuilder.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// --- 1. VERİTABANI AYARI ---
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
    }));

builder.Services.AddScoped(p =>
    p.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());

// Servisler
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ITenantService, TenantService>();

// --- 2. BLAZOR AYARLARI ---
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        options.DetailedErrors = true;
        options.DisconnectedCircuitMaxRetained = 100;
        options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
        options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(3);
        options.MaxBufferedUnacknowledgedRenderBatches = 20;
    })
    .AddHubOptions(options =>
    {
        options.MaximumReceiveMessageSize = 100 * 1024 * 1024;
        options.ClientTimeoutInterval = TimeSpan.FromMinutes(5);
        options.KeepAliveInterval = TimeSpan.FromSeconds(10);
        options.HandshakeTimeout = TimeSpan.FromMinutes(1);
        options.MaximumParallelInvocationsPerClient = 10;
    });

var app = builder.Build();

// --- 3. UYGULAMA AKIŞI ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// --- wwwroot/uploads KLASÖRÜNÜ GARANTİLE ---
var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
if (!Directory.Exists(uploadDirectory)) Directory.CreateDirectory(uploadDirectory);

foreach (var sub in new[] { "logos", "slider", "news", "announcements", "gallery" })
{
    var subPath = Path.Combine(uploadDirectory, sub);
    if (!Directory.Exists(subPath)) Directory.CreateDirectory(subPath);
}

app.UseAntiforgery();

// =============================================================
// ÖRNEK İÇERİK EKLEME
// =============================================================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    var anasayfa = db.Pages.IgnoreQueryFilters().FirstOrDefault(p => p.Slug == "anasayfa");
    if (anasayfa != null)
    {
        db.Pages.Remove(anasayfa);
        db.SaveChanges();
    }

    var pages = db.Pages.IgnoreQueryFilters().ToList();
    var updated = 0;

    foreach (var page in pages)
    {
        var content = GetSampleContent(page.Slug);
        if (!string.IsNullOrEmpty(content))
        {
            page.ContentJson = content;
            updated++;
        }
    }

    if (updated > 0)
    {
        db.SaveChanges();
        Console.WriteLine($">>> {updated} sayfa güncellendi!");
    }
}
// =============================================================

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

// --- ÖRNEK İÇERİK FONKSİYONU ---
static string? GetSampleContent(string? slug)
{
    if (string.IsNullOrEmpty(slug)) return null;

    return slug switch
    {
        "ana-sayfa" => JsonSerializer.Serialize(new
        {
            PageType = "Standard",
            HasSlider = true,
            Sliders = new[]
            {
                new { ImageUrl = "https://picsum.photos/seed/slider1/1920/600", Title = "Bartın Sınav İleri Kurs", LinkUrl = "" },
                new { ImageUrl = "https://picsum.photos/seed/slider2/1920/600", Title = "Uzman Kadromuzla Başarıya Ulaşın", LinkUrl = "/view/hakkimizda" },
                new { ImageUrl = "https://picsum.photos/seed/slider3/1920/600", Title = "2024-2025 Kayıtları Başladı!", LinkUrl = "/view/iletisim" },
                new { ImageUrl = "https://picsum.photos/seed/slider4/1920/600", Title = "YKS'de %95 Başarı Oranı", LinkUrl = "/view/hizmetlerimiz" }
            },
            HasNews = true,
            NewsItems = new[]
            {
                new { Title = "YKS'de Büyük Başarı!", Summary = "Öğrencilerimiz 2024 YKS'de rekor kırdı. 150 öğrencimiz ilk 10.000'e girdi.", ImageUrl = "https://picsum.photos/seed/news1/400/250", LinkUrl = "", Content = "" },
                new { Title = "Ödül Törenimiz Gerçekleşti", Summary = "Başarılı öğrencilerimize ödüllerini takdim ettik.", ImageUrl = "https://picsum.photos/seed/news2/400/250", LinkUrl = "", Content = "" },
                new { Title = "Yeni Şubemiz Açıldı", Summary = "Bartın merkezdeki yeni şubemiz hizmete girdi.", ImageUrl = "https://picsum.photos/seed/news3/400/250", LinkUrl = "", Content = "" },
                new { Title = "Ücretsiz Deneme Sınavı", Summary = "Bu hafta sonu ücretsiz YKS deneme sınavı düzenliyoruz.", ImageUrl = "https://picsum.photos/seed/news4/400/250", LinkUrl = "", Content = "" },
                new { Title = "Yaz Kampı Başlıyor", Summary = "Yoğunlaştırılmış yaz kampı programımız başladı.", ImageUrl = "https://picsum.photos/seed/news5/400/250", LinkUrl = "", Content = "" },
                new { Title = "Online Eğitim Platformu", Summary = "Yeni online eğitim platformumuz aktif!", ImageUrl = "https://picsum.photos/seed/news6/400/250", LinkUrl = "", Content = "" }
            },
            HasAnnouncements = true,
            Announcements = new[]
            {
                new { Title = "2024-2025 Kayıtları Başladı", ImageUrl = "https://picsum.photos/seed/ann1/400/250", LinkUrl = "/view/iletisim", Content = "Erken kayıt avantajlarından yararlanın! %20 indirim fırsatı." },
                new { Title = "Deneme Sınavı Takvimi", ImageUrl = "https://picsum.photos/seed/ann2/400/250", LinkUrl = "/view/duyurular", Content = "Kasım ayı deneme sınavı 15 Kasım'da yapılacaktır." },
                new { Title = "Veli Toplantısı", ImageUrl = "https://picsum.photos/seed/ann3/400/250", LinkUrl = "", Content = "20 Kasım Çarşamba saat 19:00'da veli toplantımız var." },
                new { Title = "Kış Kampı Kayıtları", ImageUrl = "https://picsum.photos/seed/ann4/400/250", LinkUrl = "", Content = "Yarıyıl tatili kamp programı kayıtları başladı!" },
                new { Title = "Burs İmkanları", ImageUrl = "https://picsum.photos/seed/ann5/400/250", LinkUrl = "", Content = "Başarılı öğrencilere %50'ye varan burs imkanı." },
                new { Title = "Rehberlik Semineri", ImageUrl = "https://picsum.photos/seed/ann6/400/250", LinkUrl = "", Content = "Üniversite tercih semineri bu Cumartesi!" }
            },
            HasGallery = false,
            GalleryItems = new object[] { },
            HtmlContent = "<div class='text-center py-5'><h2 class='text-primary display-5 fw-bold'>Hoş Geldiniz!</h2><p class='lead fs-5'>Bartın Sınav İleri Kurs olarak 20 yılı aşkın tecrübemizle öğrencilerimizi hayallerine kavuşturuyoruz.</p><div class='row mt-5 g-4'><div class='col-md-4'><div class='p-4 bg-light rounded shadow-sm'><h3 class='text-success display-6 fw-bold'>10.000+</h3><p class='fs-5'>Mezun Öğrenci</p></div></div><div class='col-md-4'><div class='p-4 bg-light rounded shadow-sm'><h3 class='text-primary display-6 fw-bold'>50+</h3><p class='fs-5'>Uzman Öğretmen</p></div></div><div class='col-md-4'><div class='p-4 bg-light rounded shadow-sm'><h3 class='text-warning display-6 fw-bold'>%95</h3><p class='fs-5'>Başarı Oranı</p></div></div></div></div>",
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
            HtmlContent = "<div class='container'><h2 class='display-6 fw-bold'>Hakkımızda</h2><p class='lead'>Bartın Sınav İleri Kurs, 2004 yılından bu yana Bartın'da eğitim hizmeti vermektedir.</p><hr><h4>Tarihçemiz</h4><p>Kurumumuz, küçük bir dershaneden başlayarak bugün Bartın'ın en köklü eğitim kurumlarından biri haline gelmiştir.</p><h4>Değerlerimiz</h4><ul><li>Öğrenci odaklı eğitim anlayışı</li><li>Sürekli gelişim ve yenilikçilik</li><li>Akademik mükemmellik</li><li>Etik ve şeffaflık</li></ul></div>",
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
            HtmlContent = "<div class='container'><h2 class='display-6 fw-bold'>Yönetim Kadromuz</h2><div class='row mt-4'><div class='col-md-4 text-center mb-4'><div class='p-4 bg-light rounded shadow-sm'><img src='https://picsum.photos/seed/person1/150/150' class='rounded-circle mb-3' style='width:120px;height:120px;object-fit:cover'><h5>Ahmet Yılmaz</h5><p class='text-muted'>Kurucu & Genel Müdür</p></div></div><div class='col-md-4 text-center mb-4'><div class='p-4 bg-light rounded shadow-sm'><img src='https://picsum.photos/seed/person2/150/150' class='rounded-circle mb-3' style='width:120px;height:120px;object-fit:cover'><h5>Fatma Demir</h5><p class='text-muted'>Eğitim Koordinatörü</p></div></div><div class='col-md-4 text-center mb-4'><div class='p-4 bg-light rounded shadow-sm'><img src='https://picsum.photos/seed/person3/150/150' class='rounded-circle mb-3' style='width:120px;height:120px;object-fit:cover'><h5>Mehmet Kaya</h5><p class='text-muted'>Akademik Danışman</p></div></div></div></div>",
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
            HtmlContent = "<div class='container'><div class='row'><div class='col-md-6 mb-4'><div class='card h-100 border-primary shadow'><div class='card-header bg-primary text-white'><h4 class='mb-0'>🎯 Misyonumuz</h4></div><div class='card-body'><p class='fs-5'>Öğrencilerimize kaliteli eğitim vererek onları hayallerine kavuşturmak.</p></div></div></div><div class='col-md-6 mb-4'><div class='card h-100 border-success shadow'><div class='card-header bg-success text-white'><h4 class='mb-0'>🔭 Vizyonumuz</h4></div><div class='card-body'><p class='fs-5'>Türkiye'nin en başarılı eğitim kurumları arasında yer almak.</p></div></div></div></div></div>",
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
                new { Title = "2024-2025 Kayıtları Başladı", ImageUrl = "https://picsum.photos/seed/duyuru1/400/250", LinkUrl = "/view/iletisim", Content = "Yeni eğitim dönemi kayıtlarımız başlamıştır." },
                new { Title = "Deneme Sınavı Takvimi", ImageUrl = "https://picsum.photos/seed/duyuru2/400/250", LinkUrl = "", Content = "Kasım ayı deneme sınavı 15 Kasım'da." },
                new { Title = "Veli Toplantısı", ImageUrl = "https://picsum.photos/seed/duyuru3/400/250", LinkUrl = "", Content = "20 Kasım saat 19:00'da veli toplantısı." },
                new { Title = "Kış Kampı", ImageUrl = "https://picsum.photos/seed/duyuru4/400/250", LinkUrl = "", Content = "Yarıyıl tatili kamp programı başlıyor!" }
            },
            HasGallery = false,
            GalleryItems = new object[] { },
            HtmlContent = "<div class='container'><h2 class='display-6 fw-bold'>📢 Duyurular</h2><p class='text-muted fs-5'>Güncel duyurularımız</p><hr></div>",
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
            HtmlContent = "<div class='container'><h2 class='display-6 fw-bold'>🧭 Rehberlik Hizmetlerimiz</h2><p class='lead'>Uzman kadromuzla her aşamada yanınızdayız.</p><div class='row mt-4'><div class='col-md-6 mb-3'><div class='card border-info h-100 shadow-sm'><div class='card-body'><h5>📚 Akademik Rehberlik</h5><p>Ders çalışma teknikleri ve verimli öğrenme.</p></div></div></div><div class='col-md-6 mb-3'><div class='card border-warning h-100 shadow-sm'><div class='card-body'><h5>🎯 Kariyer Danışmanlığı</h5><p>Tercih döneminde profesyonel destek.</p></div></div></div></div></div>",
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
            HtmlContent = "<div class='container'><h2 class='display-6 fw-bold'>📖 Eğitim Programlarımız</h2><div class='row mt-4'><div class='col-md-3 mb-4'><div class='card h-100 text-center shadow'><img src='https://picsum.photos/seed/yks/300/200' class='card-img-top'><div class='card-body'><h5 class='text-primary'>YKS Hazırlık</h5><p class='small'>TYT ve AYT kursları</p></div></div></div><div class='col-md-3 mb-4'><div class='card h-100 text-center shadow'><img src='https://picsum.photos/seed/lgs/300/200' class='card-img-top'><div class='card-body'><h5 class='text-success'>LGS Hazırlık</h5><p class='small'>8. sınıf programı</p></div></div></div><div class='col-md-3 mb-4'><div class='card h-100 text-center shadow'><img src='https://picsum.photos/seed/kpss/300/200' class='card-img-top'><div class='card-body'><h5 class='text-warning'>KPSS Hazırlık</h5><p class='small'>Memur adayları için</p></div></div></div><div class='col-md-3 mb-4'><div class='card h-100 text-center shadow'><img src='https://picsum.photos/seed/dgs/300/200' class='card-img-top'><div class='card-body'><h5 class='text-info'>DGS Hazırlık</h5><p class='small'>Dikey geçiş</p></div></div></div></div></div>",
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
                City = "Bartın",
                District = "Merkez",
                MapEmbedCode = "<iframe src='https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d47982!2d32.31!3d41.63!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x0!2zQmFydMSxbg!5e0!3m2!1str!2str' width='100%' height='350' style='border:0;' allowfullscreen='' loading='lazy'></iframe>",
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
                new { ImageUrl = "https://picsum.photos/seed/g1/400/300", Title = "Kurs Binamız", Description = "Modern eğitim ortamı" },
                new { ImageUrl = "https://picsum.photos/seed/g2/400/300", Title = "Derslikler", Description = "Akıllı tahta donanımlı" },
                new { ImageUrl = "https://picsum.photos/seed/g3/400/300", Title = "Kütüphane", Description = "Zengin kaynak arşivi" },
                new { ImageUrl = "https://picsum.photos/seed/g4/400/300", Title = "Laboratuvar", Description = "Bilgisayar lab." },
                new { ImageUrl = "https://picsum.photos/seed/g5/400/300", Title = "Mezuniyet", Description = "2024 töreni" },
                new { ImageUrl = "https://picsum.photos/seed/g6/400/300", Title = "Başarılar", Description = "Ödüller" },
                new { ImageUrl = "https://picsum.photos/seed/g7/400/300", Title = "Etkinlikler", Description = "Sosyal aktiviteler" },
                new { ImageUrl = "https://picsum.photos/seed/g8/400/300", Title = "Seminerler", Description = "Motivasyon" }
            },
            HtmlContent = "<div class='container'><h2 class='display-6 fw-bold'>📸 Galeri</h2><hr></div>",
            Contact = new { }
        }),

        _ => null
    };
}