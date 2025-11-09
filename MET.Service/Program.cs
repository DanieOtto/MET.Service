using System.Text;
using MET.Service.Application.Interfaces;
using MET.Service.Application.Services;
using MET.Service.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MET.Service.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

// Add authentication services
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
            ValidIssuer = config["Jwt:Issuer"],
            ValidAudience = config["Jwt:Audience"]
        };
    });

// Add authorization services
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Add Swagger
builder.Services.AddOpenApi();

// Add application services
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();

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

app.UseAuthentication();
app.UseAuthentication();

// Configure the HTTP request pipeline.
app.MapControllers();

// Enable middleware to serve generated Swagger as a JSON endpoint (if the environment is Development).
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "MET API V1");
    });
}

app.Run();