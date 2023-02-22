using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using UrlShortener.Data.Data;
using UrlShortener.Data.Services;
using UrlShortener.Domain.Interfaces;
using UrlShortener.WebApplication.Validators;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);

builder.Services.AddValidatorsFromAssemblyContaining<CreateShortUrlValidator>(ServiceLifetime.Transient);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DataContext>
    (o => o.UseInMemoryDatabase("UrlShortenerDatabase"));
builder.Services.AddScoped<IUrlShortenerRepository, UrlShortenerRepository>();

var app = builder.Build();
app.UseSerilogRequestLogging();

if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Home/Error");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();