using System.Collections.Concurrent;
using Notifications.Application.Services.SignalR;

namespace Notifications.Infrastructure.SignalR;

public class UserConnectionManager : IUserConnectionManager
{
    private readonly ConcurrentDictionary<Guid, HashSet<string>> _userConnections = new();

    public void AddConnection(Guid userId, string connectionId)
    {
        _userConnections.AddOrUpdate(
            userId,
            _ => new HashSet<string> { connectionId },
            (_, connections) =>
            {
                connections.Add(connectionId);
                return connections;
            });
    }

    public void RemoveConnection(Guid userId, string connectionId)
    {
        if (_userConnections.TryGetValue(userId, out var connections))
        {
            connections.Remove(connectionId);
            if (connections.Count == 0)
            {
                _userConnections.TryRemove(userId, out _);
            }
        }
    }

    public bool IsUserConnected(Guid userId) => _userConnections.ContainsKey(userId);
    
}
