using Back.Context;
using Back.Services.AuthService;
using Back.Services.ProdutoService;
using Back.Services.VendaService;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Back.Configurations;

public static class ServiceConfiguration
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        var firebaseValidIssuer = Environment.GetEnvironmentVariable("FIREBASE_VALID_ISSUER");
        var firebaseAudience = Environment.GetEnvironmentVariable("FIREBASE_AUDIENCE");
        var firebaseCredentials = Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS");

        FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromJson(firebaseCredentials)
        });

        services.AddScoped<IAuthInterface, AuthService>();
        services.AddScoped<IProdutoInterface, ProdutoService>();
        services.AddScoped<IVendaInterface, VendaService>();

        services.AddHttpClient<string>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.Authority = firebaseValidIssuer;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = firebaseValidIssuer,
                ValidAudience = firebaseAudience,
            };
        });

        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Por favor, insira o token JWT com o prefixo 'Bearer '",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
    }
}