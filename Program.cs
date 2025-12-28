using CoreBuilder.Components;
using CoreBuilder.Data;
using CoreBuilder.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. VERİTABANI AYARI (ZIRHLI VERSİYON) ---
// EnableRetryOnFailure: Bağlantı anlık koparsa sistem çökmez, otomatik tekrar dener.
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
    }));

// Eski kodların çalışması için (Geriye uyumluluk)
builder.Services.AddScoped(p =>
    p.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());

// Diğer Servisler
builder.Services.AddHttpContextAccessor();

// --- KRİTİK: BURASI KESİNLİKLE 'AddTransient' OLMALI ---
builder.Services.AddTransient<ITenantService, TenantService>();

// --- 2. BLAZOR AYARLARI ---
builder.Services.AddRazorComponents()
    // 👇 BURAYA DİKKAT: Hata detaylarını açtık. Artık sarı çubuk yerine gerçek hatayı göreceğiz.
    .AddInteractiveServerComponents(options => options.DetailedErrors = true)
    .AddHubOptions(options =>
    {
        // Dosya yüklerken "Rejoining" hatasını engellemek için limiti artırdık (100 MB).
        options.MaximumReceiveMessageSize = 100 * 1024 * 1024;

        options.ClientTimeoutInterval = TimeSpan.FromMinutes(2);
        options.KeepAliveInterval = TimeSpan.FromSeconds(15);
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
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();