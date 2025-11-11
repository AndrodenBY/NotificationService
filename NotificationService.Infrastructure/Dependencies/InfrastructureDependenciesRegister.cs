using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using NotificationService.Application.Interfaces;
using NotificationService.Infrastructure.Helpers;
using NotificationService.Infrastructure.Options;
using NotificationService.Infrastructure.Repositories;
using NotificationService.Infrastructure.Services;

namespace NotificationService.Infrastructure.Dependencies;

public static class InfrastructureDependenciesRegister
{
    public static void RegisterNotificationServiceDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddEmailServices(configuration);
        services.AddMassTransitConsumers(configuration);
        
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        
        var connectionString = configuration["MongoDbConnectionString"];
        var databaseName = configuration["MongoDbNotificationServiceDatabase"];
        var collectionName = configuration["MongoDbNotificationServiceCollection"];
        
        services.AddSingleton(MongoCollectionInitializer.InitializeCollection(connectionString!, databaseName!, collectionName!))
                .AddScoped<ILogRepository, MongoLogRepository>()
                .AddScoped<IEmailService, EmailService>()
                .Decorate<IEmailService, EmailServiceLoggingDecorator>();
    }
}
