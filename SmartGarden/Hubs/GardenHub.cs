using Microsoft.AspNetCore.SignalR;
using SmartGarden.Models;

namespace SmartGarden.Hubs
{
    public class GardenHub : Hub
    {
        private readonly IHubContext<GardenHub> _hubContext;

        public GardenHub(IHubContext<GardenHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task BroadcastGardenState(GardenParameters message)
        {
            // Broadcast the message to all connected clients
            await _hubContext.Clients.All.SendAsync("GardenParameters", message);
        }

        public async Task BroadcastGardenImage(object message) // TODO: definirati koja je slika formata
        {
            // Broadcast the image to all connected clients
            await _hubContext.Clients.All.SendAsync("GardenImage", message);
        }
    }
}
