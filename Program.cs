using System.Text;
using CourseManagementAPI.Data;
using CourseManagementAPI.Services.Implementations;
using CourseManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ── CORS ─────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ── Database ────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// ── Services (Dependency Injection) ─────────────────────────────────────────
builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<IStudentService,    StudentService>();
builder.Services.AddScoped<ICourseService,     CourseService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IAuthService,       AuthService>();

// ── JWT Authentication ───────────────────────────────────────────────────────
var jwtKey    = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = jwtIssuer,
            ValidAudience            = jwtAudience,
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// ── Controllers ──────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── Swagger ──────────────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title   = "Course Management API",
        Version = "v1",
        Description = "ASP.NET Core Web API for managing courses, students, and instructors."
    });

    // Allow JWT in Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.Http,
        Scheme       = "Bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header,
        Description  = "Enter your JWT token. Example: Bearer eyJhbGci..."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// ── Build ────────────────────────────────────────────────────────────────────
var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization(); 
app.UseStaticFiles();
app.MapControllers();

app.Run();