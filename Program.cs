using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YemekTarifiWeb.Data;
using YemekTarifiWeb.Areas.Identity.Data; // ApplicationUser için gerekli

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabanı bağlamını (DbContext) servis olarak ekle
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Identity servisini ApplicationUser ile yapılandır
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // E-posta onayı gerektirme
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// 3. MVC ve Razor Pages desteğini ekle
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Middleware pipeline ayarları
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Kimlik doğrulama
app.UseAuthorization();  // Yetkilendirme

// MVC rotasını ayarla
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Identity için Razor Pages’i ekle
app.MapRazorPages();

app.Run();
