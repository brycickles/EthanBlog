using EthanBlog.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using EthanBlog.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using EthanBlog.BusinessManagers.Interfaces;
using EthanBlog.Service.Interfaces;
using EthanBlog.BusinessManagers;
using EthanBlog.Service;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authorization;
using EthanBlog.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//make services able to be injected
builder.Services.AddScoped<IBlogBusinessManager, BlogBusinessManager>();
builder.Services.AddScoped<IAdminBusinessManager, AdminBusinessManager>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<DbContext, ApplicationDbContext>(f => f.GetService<ApplicationDbContext>());

//Add file provider service
builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

//Add service for authorization 
builder.Services.AddTransient<IAuthorizationHandler, BlogAuthorizationHandler>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
