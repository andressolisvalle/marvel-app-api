using Application.Helpers;
using Application.Interface;
using Application.Mappings;
using Application.Services;
using Domain.Repositories;
using FluentValidation.AspNetCore;
using Infrastructure.MarvelApi;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy
            .WithOrigins("http://localhost:4200") // ?? Permitimos solo tu frontend
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

builder.Services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());


// Configurar DbContext
builder.Services.AddDbContext<MarvelDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Infrastructure")));

builder.Services.AddSingleton<JwtTokenGenerator>();

//repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFavoriteListRepository, FavoriteListRepository>();
builder.Services.AddHttpClient<MarvelApiClient>();

//servicios
services.AddTransient<IUserSevice, UserService>();
services.AddTransient<IComicService, ComicService>();
services.AddTransient<IFavoriteService, FavoriteService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


// Configurar Autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // puedes activarlo si quieres validar el emisor
            ValidateAudience = false, // puedes activarlo si quieres validar el público
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Configurar autorización
builder.Services.AddAuthorization();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MarvelApp API", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando el esquema Bearer. \r\n\r\n Introduzca 'Bearer' seguido de su token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http, 
        Scheme = "bearer", 
        BearerFormat = "JWT"
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
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();
app.UseMiddleware<Api.Middleware.ValidationExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
