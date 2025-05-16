using Application.Commands.Owners;
using Application.Commands.Properties;
using Application.Commands.PropertyTraces;
using Application.Interfaces;
using Application.Validators.Owners;
using Application.Validators.Properties;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.OpenApi.Models;
using Shared.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

// MongoDB
builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));
builder.Services.AddSingleton<MongoDbContext>();


// Firebase
builder.Services.Configure<FirebaseSettings>(builder.Configuration.GetSection("FirebaseSettings"));
builder.Services.AddSingleton<IFileStorageService, FirebaseStorageService>();

// Servicios, CQRS, Validadores
builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(CreateOwnerCommand).Assembly); });
builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(CreatePropertyWithImageCommand).Assembly); });
builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(CreatePropertyTraceCommand).Assembly); });

builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();
builder.Services.AddScoped<IPropertyTraceRepository, PropertyTraceRepository>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateOwnerValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateOwnerValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreatePropertyValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AddPropertyImageValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdatePropertyValidator>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Real Estate API",
        Version = "v1",
        Description = "API de gestión inmobiliaria con Clean Architecture",
        Contact = new OpenApiContact
        {
            Name = "Soporte Técnico",
            Email = "soporte@realestate.com"
        }
    });
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "API.xml"));
});

var app = builder.Build();

app.UseMiddleware<API.Middlewares.ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RealEstate API v1");
    c.DocumentTitle = "RealEstate API Docs";
    c.RoutePrefix = "swagger";
    c.DisplayRequestDuration();
});
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();