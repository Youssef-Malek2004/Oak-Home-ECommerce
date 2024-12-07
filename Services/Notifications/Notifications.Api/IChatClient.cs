namespace Notifications.Api;

public interface IChatClient
{
    Task ReceiveNotification(string message);
}