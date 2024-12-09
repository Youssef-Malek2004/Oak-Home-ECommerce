using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Notifications.Application.CQRS.Commands;
using Notifications.Application.Services.SignalR;

namespace Notifications.Infrastructure.SignalR;

[Authorize]
public sealed class ChatHub(IMediator mediator,
    INotificationService notificationService,
    IUserConnectionManager userConnectionManager) : Hub<IChatClient>
{
    public async Task SendMessage(string message)
    {
        await Clients.All.ReceiveNotification($"{Context.ConnectionId}: {message}");
    }

    public override async Task OnConnectedAsync()
    {
        var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier) ?? 
                          Context.User?.FindFirst("sub");

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            await Clients.Caller.ReceiveNotification("Connection failed: Valid UserId is required.");
            Context.Abort();
            return;
        }
        
        Console.WriteLine($"User with Id: {userId} Connected");
        
        userConnectionManager.AddConnection(userId, Context.ConnectionId);
        await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());

        await notificationService.SendUndeliveredNotificationsAsync(userId);
        await notificationService.SendUnreadNotificationsAsync(userId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier) ?? 
                          Context.User?.FindFirst("sub");

        if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
        {
            userConnectionManager.RemoveConnection(userId, Context.ConnectionId);
            Console.WriteLine($"User with Id: {userId} Disconnected");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId.ToString());
        }
        

        await base.OnDisconnectedAsync(exception);
    }

    public async Task MarkNotificationAsRead(Guid notificationId)
    {
        var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier) ?? 
                          Context.User?.FindFirst("sub");

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            await Clients.Caller.ReceiveNotification("Error: Valid UserId is required.");
            return;
        }

        var result = await mediator.Send(new MarkNotificationAsReadCommand(userId, notificationId));

        if (result.IsSuccess)
        {
            Console.WriteLine($"Notification {notificationId} marked as read.");
        }
        else
        {
            Console.WriteLine(
                $"Error marking notification {notificationId} as read: {result.Error}");
        }
    }
}
