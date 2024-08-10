using Back.Services.AuthService;
using FirebaseAdmin;
using Back.Context;
using Back.Services.ProdutoService;
using Back.Services.VendaService;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
var firebaseValidIssuer = Environment.GetEnvironmentVariable("FIREBASE_VALID_ISSUER");
var firebaseAudience = Environment.GetEnvironmentVariable("FIREBASE_AUDIENCE");
var firebaseTokenUri = Environment.GetEnvironmentVariable("FIREBASE_TOKEN_URI");
var firebaseCredentials = Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS");

FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromJson(firebaseCredentials)
});

builder.Services.AddScoped<IAuthInterface, AuthService>();
builder.Services.AddScoped<IProdutoInterface, ProdutoService>();
builder.Services.AddScoped<IVendaInterface, VendaService>();

builder.Services.AddHttpClient<string>();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8080);
    serverOptions.ListenAnyIP(8081);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.Authority = firebaseValidIssuer;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = firebaseValidIssuer,
        ValidAudience = firebaseAudience,
    };
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

// Botão de Authorization adicionado no Swagger
builder.Services.AddSwaggerGen(c =>
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

var app = builder.Build();

// Aplica as migrações do banco de dados
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        x.SwaggerEndpoint("/swagger/v1/swagger.json", "Versão 1.0");
        x.InjectStylesheet("/css/swaggerDark.css");
        x.RoutePrefix = string.Empty;
    });
}

app.UseStaticFiles();

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();