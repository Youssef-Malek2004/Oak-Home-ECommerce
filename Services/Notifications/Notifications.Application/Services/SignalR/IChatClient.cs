namespace Notifications.Application.Services.SignalR;

public interface IChatClient
{
    Task ReceiveNotification(string message);
}