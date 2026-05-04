using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using POS.Application.Interfaces;
using POS.Application.Services;
using POS.Infrastructure.Data;
using POS.Infrastructure.Identity;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Register services before builder.Build()

builder.Services.AddControllers();
builder.Services.AddOpenApi();

DotNetEnv.Env.Load();

var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

//Validation
if (string.IsNullOrWhiteSpace(jwtKey))
    throw new InvalidOperationException("JWT_KEY is missing in .env");

if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("DB_CONNECTION is missing in .env");

if (string.IsNullOrWhiteSpace(jwtIssuer))
    throw new InvalidOperationException("JWT_ISSUER is missing in .env");

if (string.IsNullOrWhiteSpace(jwtAudience))
    throw new InvalidOperationException("JWT_AUDIENCE is missing in .env");

var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddDbContext<POSDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<POSDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("UserOnly", policy =>
        policy.RequireRole("User"));
});

builder.Services.AddScoped<IAuthService, AuthService>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient", policy =>
    {
        policy.WithOrigins("https://localhost:54438")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

//Console.WriteLine("Conn: " + connectionString);

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await POS.Infrastructure.Data.DbSeeder.SeedAsync(services);
}

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowClient");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
