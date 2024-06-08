using API.Context;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder
    .Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/IdentityController/Login";
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DBContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("UserRegistry"));
});

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<ValidationService>();
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve
);
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

// Use the localization middleware
var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);

app.MapControllers();

app.Run();
