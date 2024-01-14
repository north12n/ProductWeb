global using Microsoft.EntityFrameworkCore;
global using ProductWeb.Models;
global using Microsoft.AspNetCore.Identity;
global using ProductWeb.Date;
using ProductWeb.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProductContext>();


//เปลี่ยน
builder.Services.AddDefaultIdentity<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
}).AddRoles<IdentityRole>().AddEntityFrameworkStores<ProductContext>();
      //เพิ่มมา ตัว Services จัดการ Role



// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRoleService, RoleService>();   //เพิ่ม AddScoped =======================

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
