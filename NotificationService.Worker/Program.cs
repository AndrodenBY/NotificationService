using NotificationService.Infrastructure.Dependencies;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddUserSecrets<Program>();
builder.Services.RegisterNotificationServiceDependencies(builder.Configuration);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var host = builder.Build();

host.Run();
