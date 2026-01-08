using System.Text;
using MET.Service.Application.Interfaces;
using MET.Service.Application.Services;
using MET.Service.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MET.Service.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi;

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

// Add Swashbuckle swagger services
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MET API", Version = "v1" });

//    // Define the BearerAuth scheme for the UI
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer {token}' or just the token.", 
//        Name = "Authorization",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.Http,
//        Scheme = "bearer",
//        BearerFormat = "JWT"
//    });

//    // Require the Bearer scheme for all operations so the UI will send the header
//    //c.AddSecurityRequirement(new OpenApiSecurityRequirement
//    //{
//    //    {
//    //        new OpenApiSecurityScheme
//    //        {
//    //            Description = "",
//    //            Reference = new OpenApiReference
//    //            {
//    //                Type = ReferenceType.SecurityScheme,
//    //                Id = "Bearer"
//    //            }
//    //        },
//    //        new string[] { }
//    //    }
//    //});
//});

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

// Enable Swagger UI
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(options =>
//    {
//        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MET API V1");
//    });
//}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();