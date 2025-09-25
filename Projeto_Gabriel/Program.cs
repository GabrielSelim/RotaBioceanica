using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using EvolveDb;
using Serilog;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Projeto_Gabriel.Shared.Configurations;
using Projeto_Gabriel.Application.Hypermedia.Filters;
using Projeto_Gabriel.Application.Extensions;
using Projeto_Gabriel.Infrastructure.Extensions;
using Projeto_Gabriel.Model.Context;
using Asp.Versioning;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

var appName = "Projeto de Gabriel Sanz";
var appVersion = "v1";
var descricao = "Está API foi implementada por mim Gabriel Sanz a fim de demonstrar meus conhecimentos em RESTFul APi";

// Carrega as configurações do appsettings.json e do arquivo específico do ambiente
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Transformando os End-Point em letras minúsculas
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Configurando as Validações do Token
var tokenConfigurations = new TokenConfiguration();
new ConfigureFromConfigurationOptions<TokenConfiguration>(
    builder.Configuration.GetSection("TokenConfiguration"))
    .Configure(tokenConfigurations);

builder.Services.AddSingleton(tokenConfigurations);

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
        ValidIssuer = tokenConfigurations.Issuer,
        ValidAudience = tokenConfigurations.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret)),
        NameClaimType = ClaimTypes.NameIdentifier
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json; charset=utf-8";
            var result = new { message = "Você não está autenticado." };
            return context.Response.WriteAsJsonAsync(result);
        },
        OnForbidden = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json; charset=utf-8";
            var result = new { message = "Você não tem permissão necessária para acessar este endpoint." };
            return context.Response.WriteAsJsonAsync(result);
        }
    };
});

builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser().Build());

    // Políticas baseadas em roles
    auth.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    auth.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

// Inserindo o Cors
builder.Services.AddCors(options => options.AddDefaultPolicy(builder =>
{
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
}));

// Add services to the container.
builder.Services.AddControllers();

// Configurando o API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true; // Assume uma versão padrão se não for especificada
    options.DefaultApiVersion = new ApiVersion(1, 0); // Define a versão padrão como 1.0
    options.ReportApiVersions = true; // Inclui informações de versão no cabeçalho da resposta
    options.ApiVersionReader = new UrlSegmentApiVersionReader(); // Lê a versão da URL (v{version})
});

// Configurando o Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(appVersion, new OpenApiInfo
    {
        Title = appName,
        Version = appVersion,
        Description = descricao,
        Contact = new OpenApiContact
        {
            Name = "Gabriel Sanz",
            Url = new Uri("https://github.com/GabrielSelim")
        }
    });

    // Configuração de segurança para JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor insira o token JWT com o prefixo 'Bearer '",
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
            new string[] {}
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<ErrorResponseExample>();

// Configuração do Banco de Dados
var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];

builder.Services.AddDbContext<MySQLContext>(options => options.UseMySql(
    connection,
    new MySqlServerVersion(new Version(8, 0, 36))
    ));

if (builder.Environment.IsDevelopment())
{
    MigrateDatabase(connection);
}

builder.Services.AddMvc(options =>
{
    options.RespectBrowserAcceptHeader = true;
    options.FormatterMappings.SetMediaTypeMappingForFormat("xml", "application/xml");
    options.FormatterMappings.SetMediaTypeMappingForFormat("json", "application/json");
}).AddXmlSerializerFormatters();

// Configurando HyperMedia
var filterOptions = new HyperMediaFilterOptions();
builder.Services.AddEnrichers(filterOptions);
builder.Services.AddSingleton(filterOptions);

// Dependency Injection 
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add Business Services
builder.Services.AddBusinessServices();

// Add Services
builder.Services.AddServices();

// Add Repository
builder.Services.AddInfrastructureRepositories();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// Adicionando o Cors
app.UseCors();

// Adicionando o Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint($"/swagger/{appVersion}/swagger.json", $"{appName} - {appVersion}");
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
});

var options = new RewriteOptions();
options.AddRedirect("^$", "swagger");
app.UseRewriter(options);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute("DefaultApi", "v{version:apiVersion}/{controller=values}/{id?}");

app.Run();

void MigrateDatabase(string? connection)
{
    try
    {
        var envolveConnection = new MySqlConnection(connection);
        var envolve = new Evolve(envolveConnection, Log.Information)
        {
            Locations = new List<string> { "db/migrations", "db/dataset" },
            IsEraseDisabled = true,
        };
        envolve.Migrate();
    }
    catch (Exception ex)
    {
        Log.Error("Database migration failed", ex);
        throw;
    }
}
public class RemoveVersionFromParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var versionParameter = operation.Parameters.SingleOrDefault(p => p.Name == "version");
        if (versionParameter != null)
        {
            operation.Parameters.Remove(versionParameter);
        }
    }
}

public class ReplaceVersionWithExactValueInPath : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var paths = new OpenApiPaths();
        foreach (var (key, value) in swaggerDoc.Paths)
        {
            paths.Add(key.Replace("v{version}", swaggerDoc.Info.Version), value);
        }
        swaggerDoc.Paths = paths;
    }
}