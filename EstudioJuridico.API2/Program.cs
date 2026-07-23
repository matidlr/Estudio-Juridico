using EstudioJuridico.API2.Services.Interfaces;
using MailKit;
using FluentValidation;
using FluentValidation.AspNetCore;
using EstudioJuridico.API2.Validators;
using Serilog;
using AspNetCoreRateLimit;
using EstudioJuridico.API2.Observers;
using EstudioJuridico.API2.Observers.Interfaces;
using EstudioJuridico.API2.Services;
using EstudioJuridico.API2.Repositories.Interfaces;
using EstudioJuridico.API2.Repositories.Implementations;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30
    )
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// DESPUÉS
var connectionString = builder.Configuration.GetConnectionString("MySQL")
    ?? Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? throw new InvalidOperationException("Falta la cadena de conexión MySQL");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 0))
    )
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["Jwt:Issuer"]
                                       ?? Environment.GetEnvironmentVariable("JWT_ISSUER"),
            ValidAudience            = builder.Configuration["Jwt:Audience"]
                                       ?? Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
            IssuerSigningKey         = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]
                    ?? Environment.GetEnvironmentVariable("JWT_KEY")
                    ?? throw new InvalidOperationException("Falta JWT Key")))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Registramos los servicios usando interfaces
builder.Services.AddScoped<IEstudioEmailService, EmailService>();
builder.Services.AddScoped<IWhatsAppService, WhatsAppService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICasoService, CasoService>();
builder.Services.Configure<HostOptions>(options =>
{
    options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});
// Observer Pattern - Notificaciones
builder.Services.AddScoped<INotificacionObserver, EmailNotificacionObserver>();
builder.Services.AddScoped<INotificacionObserver, WhatsAppNotificacionObserver>();
builder.Services.AddScoped<NotificacionManager>();

// Repository Pattern
builder.Services.AddScoped<ICasoRepository, CasoRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IActualizacionRepository, ActualizacionRepository>();
builder.Services.AddScoped<IArchivoRepository, ArchivoRepository>();

builder.Services.AddHostedService<RecordatorioService>();

builder.Services.AddValidatorsFromAssemblyContaining<CasoDTOValidator>();
builder.Services.AddFluentValidationAutoValidation();
// Rate limiting
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;
    options.StackBlockedRequests       = false;
    options.HttpStatusCode             = 429;
    options.RealIpHeader               = "X-Real-IP";
    options.GeneralRules = new List<RateLimitRule>
    {
        // Login: máximo 10 intentos por minuto
        new RateLimitRule
        {
            Endpoint = "POST:/api/auth/login",
            Period   = "1m",
            Limit    = 10
        },
        // Registro: máximo 5 intentos por minuto
        new RateLimitRule
        {
            Endpoint = "POST:/api/auth/register",
            Period   = "1m",
            Limit    = 5
        },
        // General: máximo 100 peticiones por minuto
        new RateLimitRule
        {
            Endpoint = "*",
            Period   = "1m",
            Limit    = 100
        }
    };
});
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "EstudioJuridico API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name        = "Authorization",
        Type        = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme      = "Bearer",
        BearerFormat = "JWT",
        In          = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingresá el token así: Bearer {token}"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();

app.UseMiddleware<EstudioJuridico.API2.Middleware.ErrorHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();
app.UseCors("Angular");
app.UseIpRateLimiting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseSerilogRequestLogging();

app.Run();