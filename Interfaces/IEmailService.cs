using SubsTracker.Messaging.Contracts;

namespace NotificationService.Interfaces
{
    public interface IEmailService 
    {
        Task Send<TEvent>(string recipientEmail, string subject, string templatePath, TEvent eventModel, CancellationToken cancellationToken) where TEvent : BaseEvent;
    }
}

// namespace NotificationService.Interfaces;
//
// public interface IEmailService //<T> where T : BaseEvent
// {
//     Task Send(string email, string subject, string message);
// }
