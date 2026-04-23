using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using POS.Infrastructure.Data;
using POS.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAuthorization();


var app = builder.Build();

string? connectionString = builder.Configuration.GetConnectionString("POS");

builder.Services.AddDbContext<POSDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<POSDbContext>()
    .AddDefaultTokenProviders();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
