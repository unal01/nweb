using CoreBuilder.Components;
using CoreBuilder.Data;
using CoreBuilder.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabaný ve Servisler
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantService, TenantService>();

// ... (Önceki kodlar)

// 2. Blazor ve SignalR Ayarlarý (GÜÇLENDÝRÝLMÝÞ AYARLAR)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddHubOptions(options =>
    {
        options.MaximumReceiveMessageSize = 64 * 1024 * 1024; // 64 MB Limit
        options.ClientTimeoutInterval = TimeSpan.FromMinutes(2); // 2 Dakika Zaman Aþýmý
        options.KeepAliveInterval = TimeSpan.FromSeconds(15); // Baðlantýyý canlý tut
    });

// ... (Kalan kodlar ayný)

var app = builder.Build();

// Standart Ayarlar
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