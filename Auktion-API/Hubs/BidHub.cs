using Microsoft.AspNetCore.SignalR;

namespace Auktion_API.Hubs;

public class BidHub : Hub
{
    public Task JoinLot(string lotId) {
        Console.WriteLine("User connected to hub::::::::");
        return Groups.AddToGroupAsync(Context.ConnectionId, lotId);
    }
    
    public Task LeaveLot(string lotId) {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, lotId);
    }
}