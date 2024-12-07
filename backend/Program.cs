using System.Text;
using JobHuntBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using JobHuntBackend.Models;
using JobHuntBackend.Services;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using JobHuntBackend.Middleware;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add JWT Settings configuration
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
JwtSettings jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
    ?? throw new InvalidOperationException("JWT Settings not configured");

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.SecretKey)
            ),
            ClockSkew = TimeSpan.Zero // Removes the default 5 minute clock skew
        };
    });

// Add authorization
builder.Services.AddAuthorization();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<JobHuntDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Add rate limiting
builder.Services.AddRateLimiter(options =>
{
    // General rate limit policy
    options.AddFixedWindowLimiter("GeneralLimit", opt =>
    {
        opt.PermitLimit = builder.Configuration.GetValue<int>("RateLimiting:GeneralLimit:PermitLimit");
        opt.Window = TimeSpan.FromSeconds(builder.Configuration.GetValue<int>("RateLimiting:GeneralLimit:Window"));
        opt.AutoReplenishment = true;
    });

    // Stricter auth endpoint policy
    options.AddFixedWindowLimiter("AuthLimit", opt =>
    {
        opt.PermitLimit = builder.Configuration.GetValue<int>("RateLimiting:AuthLimit:PermitLimit");
        opt.Window = TimeSpan.FromSeconds(builder.Configuration.GetValue<int>("RateLimiting:AuthLimit:Window"));
        opt.AutoReplenishment = true;
    });

    // Global rate limit exceeded handler
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        context.HttpContext.Response.Headers["Retry-After"] = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
            ? ((int)retryAfter.TotalSeconds).ToString()
            : "60";

        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            Error = "Too many requests. Please try again later.",
            RetryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out TimeSpan retryAfterSpan)
                ? (double?)retryAfterSpan.TotalSeconds
                : null
        });
    };
});

// Add CORS support
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy",
        corsBuilder =>
        {
            corsBuilder
                .WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>())
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

// Add these lines with your other service registrations
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddControllers();

// Add near the top with other service registrations
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Job Hunt API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new()
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new() { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

WebApplication app = builder.Build();

// Enable rate limiting
app.UseRateLimiter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Add before app.UseAuthentication()
app.UseCors("DefaultPolicy");

// Add after app.UseHttpsRedirection() and before other middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

// Map your controllers
app.MapControllers();

app.Run();