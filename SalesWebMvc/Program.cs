using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using SalesWebMvc.Data.Seeders;
using SalesWebMvc.Services;
using System.Globalization;
using SalesWebMvc.Data;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SalesWebMvcContext>(
    options => options
        .UseSqlServer(builder.Configuration.GetConnectionString("SalesWebMvcContext")
        ?? throw new InvalidOperationException("Connection string 'SalesWebMvcContext' not found.")));

// Location
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var culture = new List<CultureInfo> {
        new CultureInfo("en-US"),
        new CultureInfo("pt-BR")
    };
    //options.DefaultRequestCulture = new RequestCulture(culture[0]);
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = culture;
    options.SupportedUICultures = culture;
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<SellerService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<SalesRecordService>();

builder.Services.AddScoped<SeedingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.Services.CreateScope().ServiceProvider.GetRequiredService<SeedingService>().Seed();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
