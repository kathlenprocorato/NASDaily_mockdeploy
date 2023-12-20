using CITNASDaily.API.Initializer;
using CITNASDaily.Repositories.Context;
using CITNASDaily.Repositories.Contracts;
using CITNASDaily.Repositories.Repositories;
using CITNASDaily.Services.Contracts;
using CITNASDaily.Services.Services;
using CITNASDaily.Utils.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OfficeOpenXml;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    })
    .AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    // Create a Security Scheme for JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",
        Reference = new OpenApiReference
        {
            Id = "oauth2", // "oauth2" is strict in order for swagger to work correctly
            Type = ReferenceType.SecurityScheme
        }
    };

    // Add Security Definition to swagger
    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    // Add Security Requirement to swagger
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });

    options.OperationFilter<SecurityRequirementsOperationFilter>();

    // Add documentation for API
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "CITU-NAS Daily API"
    });

    //xml documentation
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddDbContext<NASContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));
});

// Register Authorization
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).Build();
});

// Register Authentication
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
            ValidIssuer = builder.Configuration["Token:Issuer"],
            ValidAudience = builder.Configuration["Token:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:Key"]))
        };
    });


builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Register custom services
ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowSpecificOrigin");
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.Seed();

app.MapControllers();

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


app.Run();



void ConfigureServices(IServiceCollection services)
{
    //cors
    services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });


    // Register automapper
    services.AddAutoMapper(
        typeof(SuperiorProfile),
        typeof(UserProfile),
        typeof(OfficeProfile),
        typeof(OASProfile),
        typeof(NASProfile),
        typeof(ActivitiesSummaryProfile),
        typeof(TimekeepingSummaryProfile),
        typeof(SuperiorEvaluationRatingProfile),
        typeof(ScheduleProfile),
        typeof(BiometricLogProfile),
        typeof(SummaryEvaluationProfile),
        typeof(ValidationProfile),
        typeof(NASSchoolYearProfile),
        typeof(DailyTimeRecordProfile)
        );

    // Register repositories
    services.AddScoped<ISuperiorRepository, SuperiorRepository>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IOASRepository, OASRepository>();
    services.AddScoped<INASRepository, NASRepository>();
    services.AddScoped<IOfficeRepository, OfficeRepository>();
    services.AddScoped<IActivitiesSummaryRepository, ActivitiesSummaryRepository>();
    services.AddScoped<ITimekeepingSummaryRepository, TimekeepingSummaryRepository>();
    services.AddScoped<ISuperiorEvaluationRatingRepository, SuperiorEvaluationRatingRepository>();
    services.AddScoped<ISummaryEvaluationRepository, SummaryEvaluationRepository>();
    services.AddScoped<IScheduleRepository, ScheduleRepository>();
    services.AddScoped<IBiometricLogRepository, BiometricLogRepository>();
    services.AddScoped<IValidationRepository, ValidationRepository>();
    services.AddScoped<IDTRRepository, DTRRepository>();
    services.AddScoped<INASSchoolYearSemesterRepository, NASSchoolYearSemesterRepository>();

    // Register services
    services.AddScoped<ISuperiorService, SuperiorService>();
    services.AddScoped<IOASService, OASService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<ITokenService, TokenService>();
    services.AddScoped<INASService, NASService>();
    services.AddScoped<IOfficeService, OfficeService>();
    services.AddScoped<IActivitiesSummaryService, ActivitiesSummaryService>();
    services.AddScoped<ITimekeepingSummaryService, TimekeepingSummaryService>();
    services.AddScoped<ISuperiorEvaluationRatingService, SuperiorEvaluationRatingService>();
    services.AddScoped<ISummaryEvaluationService, SummaryEvaluationService>();
    services.AddScoped<IScheduleService, ScheduleService>();
    services.AddScoped<IBiometricLogService, BiometricLogService>();
    services.AddScoped<IValidationService, ValidationService>();
    services.AddScoped<IDTRService, DTRService>();
}
