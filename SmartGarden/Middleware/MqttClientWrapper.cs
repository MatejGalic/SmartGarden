using Newtonsoft.Json;
using SmartGarden.Constants;
using SmartGarden.Hubs;
using SmartGarden.Models;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SmartBikeRental.Middleware;
public class MqttClientWrapper : IMqttClientWrapper
{
    private readonly string Url = "127.0.0.1";
    private readonly int Port = 1883;

    private readonly MqttClient mqttClient;
    private readonly GardenHub _hub;
    private GardenParameters _gardenParameters;


    public MqttClientWrapper(GardenHub hub)
    {
        mqttClient = new MqttClient(Url, Port, false, null, null, MqttSslProtocols.None);
        _gardenParameters = new();
        _hub = hub;
    }


    public void Connect(string clientId)
    {
        mqttClient.Connect(clientId);
        mqttClient.MqttMsgPublishReceived += OnMessageReceived;
    }

    public void Disconnect()
    {
        mqttClient.MqttMsgPublishReceived -= OnMessageReceived;
        mqttClient.Disconnect();
    }

    public void SubscribeToTopic(string topic)
    {
        mqttClient.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }
    public void SubscribeToTopics(string[] topics)
    {
        mqttClient.Subscribe(topics, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }

    public void PublishToTopic(string topic, string message)
    {
        mqttClient.Publish(topic, Encoding.ASCII.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true); // mozda ne retainat?
    }

    private void OnMessageReceived(object sender, MqttMsgPublishEventArgs e)
    {
        var message = Encoding.Default.GetString(e.Message);
        Console.WriteLine($"Received message on topic: {e.Topic}, Message: {message}");

        try
        {
            if (message != null)
            {
                switch (e.Topic)
                {
                    case Topics.ReadingFromGarden:
                        GardenParameters data = JsonConvert.DeserializeObject<GardenParameters>(message)!;
                        _gardenParameters = data;
                        _ = _hub.BroadcastGardenState(_gardenParameters);
                        break;
                    default:
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public GardenParameters GetState()
    {
        return _gardenParameters;
    }
}

