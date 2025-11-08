using EinarTask.Data;
using EinarTask.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. ÖNCE DbContext'i ekleyin
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. SONRA Identity'yi ekleyin
builder.Services.AddDefaultIdentity<User>(options =>
{
    // Þifre gereksinimleri
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    
    // Email doðrulama
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole<int>>() // Role desteði için (integer ID kullanýyorsanýz)
.AddEntityFrameworkStores<ApplicationDbContext>();

// 3. Controller servislerini ekleyin
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
// 4. AUTHENTICATION ve AUTHORIZATION middleware'lerini ekleyin
app.UseAuthentication(); // Bu çok önemli! Identity için gerekli
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// 5. Identity sayfalarýný haritalayýn (Login, Register vs.)
app.MapRazorPages();

app.Run();