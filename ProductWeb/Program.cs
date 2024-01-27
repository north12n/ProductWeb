global using Microsoft.EntityFrameworkCore;
global using ProductWeb.Models;
global using Microsoft.AspNetCore.Identity;
global using ProductWeb.Date;
global using ProductWeb.Utility;


using ProductWeb.Services;
using Microsoft.AspNetCore.Identity.UI.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProductContext>();


//เปลี่ยน
#region MyToken เพื่อเรียกใช้บริการโทเคนและบทบาทของผู้ใช้
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
}).AddDefaultTokenProviders()
.AddEntityFrameworkStores<ProductContext>();
#endregion


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRoleService, RoleService>();   //เพิ่ม AddScoped =======================


builder.Services.AddScoped<ShoppingCartService>();
#region MyPath ต้องใส่เพื่อให้จดจ าเส้นทางการ Login-Logout
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
#endregion
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//เพ่ิ่ม
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
