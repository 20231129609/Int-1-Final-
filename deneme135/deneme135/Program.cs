using deneme135.Data;  // DbContext için
using deneme135.Models;  // ApplicationUser modelini içeri aktarıyoruz
using deneme135.Repositories; // Repository sınıfını içeri aktarıyoruz
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using deneme135.Hubs; // ExamHub için gerekli

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure DbContext with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Configure Identity for Authentication and Authorization
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Parola gereksinimleri için örnek ayarlar
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    // Kullanıcı oturum süresi ayarları
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.SignIn.RequireConfirmedEmail = false; // şsteğe bağlı: E-posta onayı zorunlu değil
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Dependency Injection (DI) için Repository'leri tanıtma
builder.Services.AddScoped<ExamRepository>();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Production ortamında HSTS kullanılır
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Default routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); 

// Rolleri oluşturma
await SeedRolesAsync(app.Services);

app.MapHub<ExamHub>("/examHub");

app.Run();

/// <summary>
/// Rolleri oluturur.
/// </summary>
async Task SeedRolesAsync(IServiceProvider services)
{
    using (var scope = services.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Rolleri oluştur
        var roles = new[] { "Admin", "Student" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
