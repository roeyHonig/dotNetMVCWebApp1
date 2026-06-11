using Microsoft.AspNetCore.SignalR;

public class SimpleHub : Hub
{
    // Send a message to all connected clients
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    // Optional: just send a counter update
    public async Task SendCounter(int count)
    {
        await Clients.All.SendAsync("ReceiveCounter", count);
    }
}