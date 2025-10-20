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

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // ۲. مسیر صفحه لاگین را مشخص کنید.
        //    وقتی کاربر نیاز به لاگین داشت، به این آدرس هدایت می‌شود.
        options.LoginPath = "/login";
        // ۳. (اختیاری) می‌توانید مسیر صفحه "عدم دسترسی" را نیز مشخص کنید.
        options.AccessDeniedPath = "/access-denied";
    });

// ۴. سرویس مجوزدهی را اضافه کنید (این بخش را قبلاً داشتید).
builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

builder.Services.AddCascadingAuthenticationState(); // وضعیت احراز هویت را به کامپوننت‌ها پاس می‌دهد
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>(); // ارائه‌دهنده سفارشی شما
builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication(); // این خط را اضافه کنید
app.UseAuthorization();  // این خط را اضافه کنید


app.UseStaticFiles();
app.UseAntiforgery();



app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();