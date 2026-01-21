using MET.Service.Application.Interfaces;
using MET.Service.Application.Services;
using MET.Service.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using MET.Service.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

// Validate JWT configuration early
var jwtKey = config["Jwt:Key"];
var jwtIssuer = config["Jwt:Issuer"];
var jwtAudience = config["Jwt:Audience"];
if (string.IsNullOrWhiteSpace(jwtKey) ||
    string.IsNullOrWhiteSpace(jwtIssuer) ||
    string.IsNullOrWhiteSpace(jwtAudience))
{
    throw new InvalidOperationException("JWT configuration missing. Ensure Jwt:Key, Jwt:Issuer and Jwt:Audience are set in configuration.");
}

// Add authentication services
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience
        };
    });

// Add authorization services
builder.Services.AddAuthorization();

builder.Services.Configure<PasswordHasherOptions>(options =>
{
    // Sets the iteration count (default is 100,000 in Identity V3)
    options.IterationCount = 200000;
});

// Add Swashbuckle swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add application services
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Configure rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 20;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueLimit = 0;
    });
});

// Add database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql =>
        {
            sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            sql.CommandTimeout(60);
        }));

// Add controllers
builder.Services.AddControllers();

// Configure Middlewares
var app = builder.Build();

app.UseRateLimiter();

// Enable Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MET API V1");
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();