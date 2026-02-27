using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Pharmacy.Api.Converters;
using Pharmacy.Api.Middleware;
using Pharmacy.Application;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

try
{
    // --------------------------
    // Add services to the container
    // --------------------------
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            // Handle empty strings as null for nullable Guids
            options.JsonSerializerOptions.Converters.Add(new NullableGuidConverter());

            // Optional: Configure other JSON settings
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });

    // Add Application & Infrastructure layers
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    // --------------------------
    // Configure FluentValidation
    // --------------------------
    builder.Services.AddFluentValidationAutoValidation()
                    .AddFluentValidationClientsideAdapters();

    // --------------------------
    // Configure CORS to allow any region
    // --------------------------
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    // --------------------------
    // Configure JWT Authentication with better error handling
    // --------------------------
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var secretKey = jwtSettings["SecretKey"];
    var issuer = jwtSettings["Issuer"];
    var audience = jwtSettings["Audience"];

    // Validate JWT configuration
    if (string.IsNullOrEmpty(secretKey))
    {
        throw new InvalidOperationException("JWT SecretKey is not configured in appsettings.json");
    }
    if (string.IsNullOrEmpty(issuer))
    {
        throw new InvalidOperationException("JWT Issuer is not configured in appsettings.json");
    }
    if (string.IsNullOrEmpty(audience))
    {
        throw new InvalidOperationException("JWT Audience is not configured in appsettings.json");
    }

    Console.WriteLine($"JWT Configuration - Issuer: {issuer}, Audience: {audience}, KeyLength: {secretKey.Length}");

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false; // Set to true in production
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero, // No tolerance for expiration
            // Additional validation
            RequireExpirationTime = true,
            RequireSignedTokens = true
        };

        // Enhanced events for better debugging
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError("Authentication failed: {Error}", context.Exception.Message);
                
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    context.Response.Headers.Append("Token-Expired", "true");
                    logger.LogWarning("Token expired at {Time}", ((SecurityTokenExpiredException)context.Exception).Expires);
                }
                else if (context.Exception.GetType() == typeof(SecurityTokenInvalidSignatureException))
                {
                    logger.LogError("Invalid token signature - Secret key mismatch!");
                }
                else if (context.Exception.GetType() == typeof(SecurityTokenInvalidIssuerException))
                {
                    logger.LogError("Invalid issuer - Expected: {Expected}, Got: {Actual}", 
                        issuer, ((SecurityTokenInvalidIssuerException)context.Exception).InvalidIssuer);
                }
                else if (context.Exception.GetType() == typeof(SecurityTokenInvalidAudienceException))
                {
                    logger.LogError("Invalid audience - Expected: {Expected}", audience);
                }
                
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogWarning("JWT Challenge triggered - Path: {Path}, Error: {Error}", 
                    context.Request.Path, context.Error ?? "No error specified");
                
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var result = System.Text.Json.JsonSerializer.Serialize(new
                {
                    success = false,
                    message = "You are not authorized to access this resource",
                    error = context.Error ?? "authentication_failed",
                    errorDescription = context.ErrorDescription ?? "JWT token validation failed",
                    data = (object?)null
                });
                return context.Response.WriteAsync(result);
            },
            OnTokenValidated = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                var userName = context.Principal?.Identity?.Name ?? "Unknown";
                logger.LogInformation("Token validated successfully for user: {UserName}", userName);
                return Task.CompletedTask;
            },
            OnMessageReceived = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                var token = context.Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrEmpty(token))
                {
                    logger.LogInformation("Received token: {Token}", token.Substring(0, Math.Min(50, token.Length)) + "...");
                }
                return Task.CompletedTask;
            }
        };
    });

    builder.Services.AddAuthorization();

    // --------------------------
    // Configure Swagger with JWT and Global Authorization
    // --------------------------
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Pharmacy Information System API",
            Version = "v1",
            Description = "Pharmacy API with JWT Authentication - Use the Authorize button to add your Bearer token",
            Contact = new OpenApiContact
            {
                Name = "Pharmacy Development Team",
                Email = "support@Pharmacy.com",
                Url = new Uri("https://Pharmacy.com/support")
            },
            License = new OpenApiLicense
            {
                Name = "MIT License",
                Url = new Uri("https://opensource.org/licenses/MIT")
            }
        });

        // Add JWT Authentication to Swagger
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT"
        });

        // Global security requirement - applies to ALL endpoints
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });

        // Include XML comments for better documentation
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
        }

        // Enable annotations for better Swagger documentation
        c.EnableAnnotations();
    });

    // --------------------------
    // Add Health Checks
    // --------------------------
    builder.Services.AddHealthChecks()
        .AddDbContextCheck<Pharmacy.Infrastructure.Persistence.PharmacyDbContext>("database");

    // --------------------------
    // Build app
    // --------------------------
    var app = builder.Build();

    // --------------------------
    // Configure middleware pipeline
    // --------------------------
    
    // Global exception handling
    app.UseMiddleware<GlobalExceptionMiddleware>();

    // Enable Swagger for all environments
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pharmacy API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "Pharmacy API Documentation";
        c.DisplayRequestDuration();
        c.EnableTryItOutByDefault();
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        c.DefaultModelsExpandDepth(-1);
        c.EnableFilter();
        c.EnableDeepLinking();
        c.ConfigObject.AdditionalItems["persistAuthorization"] = true;
    });

    // Health check endpoints
    app.MapHealthChecks("/api/health");
    app.MapHealthChecks("/api/health/ready");
    app.MapHealthChecks("/health");

    // Redirect root to Swagger
    app.MapGet("/", () => Results.Redirect("/swagger"));

    // ⭐ CORS must be before Authentication and Authorization
    app.UseCors("AllowAll");

    app.UseHttpsRedirection();

    // Authentication must come before Authorization
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    // --------------------------
    // Log startup information
    // --------------------------
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("=================================================");
    logger.LogInformation("Pharmacy API is starting up...");
    logger.LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);
    logger.LogInformation("JWT Issuer: {Issuer}", issuer);
    logger.LogInformation("JWT Audience: {Audience}", audience);
    logger.LogInformation("CORS Policy: AllowAll - Accepting requests from any origin");
    logger.LogInformation("Swagger UI: /swagger");
    logger.LogInformation("Health Check: /api/health");
    logger.LogInformation("=================================================");

    // Seed the database
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<PharmacyDbContext>();

        // 1. Seed lookup data first (AppLookupMaster and AppLookupDetail) - Required for FK references
        await Pharmacy.Infrastructure.Data.LookupSeeder.SeedLookupDataAsync(context);
        logger.LogInformation("Lookup data seeded successfully");

        // 2. Seed pharmacy sample data (Branches, Users, Products, Stock, Sales)
      //  await Pharmacy.Infrastructure.Data.PharmacySeeder.SeedAsync(context);
       // logger.LogInformation("Pharmacy data seeded successfully");
    }

    app.Run();
}
catch (ReflectionTypeLoadException ex)
{
    Console.WriteLine("=================================================");
    Console.WriteLine("ReflectionTypeLoadException occurred:");
    Console.WriteLine($"Message: {ex.Message}");

    if (ex.LoaderExceptions != null)
    {
        Console.WriteLine("Loader Exceptions:");
        foreach (var loaderEx in ex.LoaderExceptions)
        {
            if (loaderEx != null)
            {
                Console.WriteLine($"  - {loaderEx.Message}");
                if (loaderEx.InnerException != null)
                {
                    Console.WriteLine($"    Inner: {loaderEx.InnerException.Message}");
                }
            }
        }
    }
    Console.WriteLine("=================================================");
    throw;
}
catch (Exception ex)
{
    Console.WriteLine("=================================================");
    Console.WriteLine($"Startup error: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
    }
    Console.WriteLine("=================================================");
    throw;
}

public partial class Program { }
