using SmartGarden.Models;

namespace SmartBikeRental.Middleware;
public interface IMqttClientWrapper
{
    public void Connect(string clientId);
    public void SubscribeToTopic(string topic);
    public void SubscribeToTopics(string[] topic);
    public void PublishToTopic(string topic, string message);
    public void Disconnect();
    public GardenParameters GetState();
}

