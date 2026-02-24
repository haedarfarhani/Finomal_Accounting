using Finomal.Application;
using Finomal.Infrastructure.IoC;
using Finomal.UI.Blazor;
using Finomal.UI.Blazor.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Dependency Injection
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddHttpContextAccessor();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "AuthToken"; // نام کوکی
        options.Cookie.HttpOnly = true; // امنیت: فقط سرور دسترسی داره
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // فقط HTTPS
        options.Cookie.SameSite = SameSiteMode.Strict; // جلوگیری از CSRF
        options.LoginPath = "/login"; // مسیر لاگین
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // انقضای کوکی (مثل JWT)
    });

// اضافه کردن JWT Validator (برای اعتبارسنجی توکن در Provider)
builder.Services.AddSingleton<IConfiguration>(builder.Configuration); // اگر کلید JWT در appsettings.json هست

// اضافه کردن Provider سفارشی برای Blazor
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddHttpClient();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
};

app.UseCookiePolicy(cookiePolicyOptions);
app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();