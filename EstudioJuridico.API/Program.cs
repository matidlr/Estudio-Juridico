
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.EntityFrameworkCore;
global using System.Security.Claims;
global using System.IdentityModel.Tokens.Jwt;
global using System.Text;
global using Microsoft.IdentityModel.Tokens;
global using MailKit.Net.Smtp;
global using MailKit.Security;
global using MimeKit;

var builder = WebApplication.CreateBuilder(args);

// Conexión a MySQL
var connectionString = builder.Configuration.GetConnectionString("MySQL");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 0))
    )
);



// JWT — autenticación con tokens
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// CORS — permite que Angular (puerto 4200) hable con la API
builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Registramos los servicios para inyección de dependencias
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CasoService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<WhatsAppService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("Angular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();