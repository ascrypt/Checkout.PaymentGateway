using System.Reflection;
using System.Threading.RateLimiting;
using Checkout.PaymentGateway.Application.Features.Payment.Commands;
using Checkout.PaymentGateway.Application.Interfaces;
using Checkout.PaymentGateway.Application.Mapping;
using Checkout.PaymentGateway.BankSimulator;
using Checkout.PaymentGateway.Infrastructure.Repositories.Payments;
using Checkout.PaymentGateway.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidatorsFromAssemblyContaining<CreatePaymentCommand>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment Gateway", Version = "v1" });
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "ApiKey must appear in header",
        Type = SecuritySchemeType.ApiKey,
        Name = "X-Api-Key",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });
    var key = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };
    var requirement = new OpenApiSecurityRequirement
    {
        { key, new List<string>() }
    };
    c.AddSecurityRequirement(requirement);
});

// Rate limiting configuration
builder.Services.AddRateLimiter(_ => _
    .AddFixedWindowLimiter("LimiterPolicy", options =>
    {
        options.PermitLimit = 1;
        // Only allow 1 request every 3 seconds
        options.Window = TimeSpan.FromSeconds(3);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        // Only queue 3 requests when we go over that limit
        options.QueueLimit = 3;
    }));

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

builder.Services.AddLogging(lb => lb.AddSerilog());

builder.Services.AddAutoMapper(typeof(PaymentMappingProfile).Assembly, Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(typeof(CreatePaymentCommandHandler).Assembly, Assembly.GetExecutingAssembly());
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IPaymentRepository, InMemoryPaymentRepository>();
builder.Services.AddSingleton<IPaymentEventHandler, PaymentEventHandler>();
builder.Services.AddTransient<IBankSimulator, BankSimulator>();

var app = builder.Build();

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.Console()
        .CreateLogger();

    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();

await app.RunAsync();

public partial class Program
{
}