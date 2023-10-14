using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using BulkyBook.Utility;
using Stripe;
using BulkyBook.DataAccess.DBInitializer;

string microsoftSecretKey = Environment.GetEnvironmentVariable("microsoft.secret");
string facebookSecretKey = Environment.GetEnvironmentVariable("facebook.secret");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options=> options.UseSqlServer(Environment.GetEnvironmentVariable("DefaultConnection")));

// Read Stripe API Key from environment variable
string stripeSecretKey = Environment.GetEnvironmentVariable("Stripe.SecretKey");
string stripePublicKey = Environment.GetEnvironmentVariable("Stripe.PublicKey");


builder.Services.Configure<StripeSettings>(options =>
{
    options.SecretKey = stripeSecretKey;
    options.PublishableKey = stripePublicKey;
});


builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});
builder.Services.AddAuthentication().AddFacebook(option =>
{
    option.AppId = "1135041287461855";
    option.AppSecret = facebookSecretKey;
});
builder.Services.AddAuthentication().AddMicrosoftAccount(option =>
{
    option.ClientId = "78bff936-96dc-423a-bd3a-a764af96eab0";
    option.ClientSecret = microsoftSecretKey;
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



builder.Services.AddScoped<IDBInitializer, DBInitializer>();

builder.Services.AddRazorPages();
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped<IEmailSender,EmailSender>();


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
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
SeedDatabase();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();


void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDBInitializer>();
        dbInitializer.Initialize();
    }
}