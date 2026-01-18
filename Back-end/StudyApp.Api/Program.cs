using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using StudyApp.Api.Data;
using StudyApp.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext (SQL Server)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? "Server=(localdb)\\MSSQLLocalDB;Database=StudyAppDb;Trusted_Connection=True;MultipleActiveResultSets=true";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Identity with GUID keys
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// JWT Authentication
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection.GetValue<string>("Key") ?? "ReplaceThisWithAStrongKey";
var issuer = jwtSection.GetValue<string>("Issuer") ?? builder.Environment.ApplicationName;
var audience = jwtSection.GetValue<string>("Audience") ?? builder.Environment.ApplicationName;
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})
    .AddJwtBearer("JwtBearer", options =>
    {
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(2)
        };
    });

builder.Services.AddAuthorization();
// Token service
builder.Services.AddScoped<StudyApp.Api.Services.ITokenService, StudyApp.Api.Services.TokenService>();
builder.Services.AddScoped<StudyApp.Api.Services.IDeckService, StudyApp.Api.Services.DeckService>();
builder.Services.AddScoped<StudyApp.Api.Services.INotesService, StudyApp.Api.Services.NotesService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseStaticFiles();
    app.UseSwaggerUI(c =>
    {
        // keep the default generated swagger JSON available
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudyApp API V1");
        // also add the custom OpenAPI YAML we created
        c.SwaggerEndpoint("/openapi.yaml", "StudyApp OpenAPI (YAML)");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Ok("StudyApp API"));

app.Run();
