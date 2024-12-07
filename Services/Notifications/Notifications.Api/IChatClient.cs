namespace Notifications.Api;

public interface IChatClient
{
    Task ReceiveMessage(string message);
}