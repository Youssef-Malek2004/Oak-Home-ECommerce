
namespace Notifications.Application.Services.SignalR;

public interface IUserConnectionManager
{
    void AddConnection(Guid userId, string connectionId);
    void RemoveConnection(Guid userId, string connectionId);
    bool IsUserConnected(Guid userId);
}