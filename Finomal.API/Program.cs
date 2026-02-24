using Finomal.API.Hubs;
using Finomal.Application;
using Finomal.Infrastructure.IoC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Dependency Injection
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("VueDevPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:3003")          
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()                            
            .SetPreflightMaxAge(TimeSpan.FromMinutes(10)); 
    });
});


var app = builder.Build();

app.UseCors("VueDevPolicy");
app.MapControllers();
app.MapHub<AccountingHub>("/AccountingHub");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.Run();