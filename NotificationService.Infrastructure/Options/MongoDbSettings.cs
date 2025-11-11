namespace NotificationService.Infrastructure.Options;

public class MongoDbSettings
{
    public string MongoDbConnectionString { get; set; } = string.Empty;
    public string MongoDbNotificationServiceDatabase { get; set; } = string.Empty;
    public string MongoDbNotificationServiceCollection { get; set; } = string.Empty;
}
