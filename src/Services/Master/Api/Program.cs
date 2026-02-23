using BuildingBlocks.Api;
using BuildingBlocks.Application;
using BuildingBlocks.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanWrite", p => p.Requirements.Add(new PermissionRequirement("master:write")));
});

builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddScoped<IPermissionService>(_ =>
    new PostgresPermissionService(builder.Configuration.GetConnectionString("Postgres")!));
builder.Services.AddSingleton<IEventPublisher>(_ =>
    new RabbitMqEventPublisher(builder.Configuration["RabbitMQ:Host"] ?? "rabbitmq"));

builder.Services.AddScoped<Master.Application.Ports.IMasterItemUseCase, Master.Application.UseCases.MasterItemUseCase>();
builder.Services.AddScoped<Master.Application.Ports.IMasterItemRepository>(sp =>
    new Master.Infrastructure.Persistence.MasterItemRepository(builder.Configuration.GetConnectionString("Postgres")!));
builder.Services.AddScoped<Master.Application.Ports.IMasterItemIntegrationPort, Master.Infrastructure.MasterItemIntegrationAdapter>();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
