using Microsoft.AspNetCore.SignalR;

namespace NeoForum.Hubs
{
    public class CounterHub : Hub
    {
        private static int Count = 0;
        public override Task OnConnectedAsync()
        {
            Count++;
            base.OnConnectedAsync();
            Clients.All.SendAsync("updateCount", Count);
            return Task.CompletedTask;
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Count--;
            base.OnDisconnectedAsync(exception);
            Clients.All.SendAsync("updateCount", Count);
            return Task.CompletedTask;
        }
    }
}
