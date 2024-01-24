using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SmartBikeRental.Middleware;
using SmartGarden.Constants;
using SmartGarden.Models;

namespace SmartGarden.Services.Garden
{
    public class GardenService : IGardenService
    {
        private readonly IMqttClientWrapper _mqttWrapper;

        public GardenService(IMqttClientWrapper mqttWrapper)
        {
            _mqttWrapper = mqttWrapper;
        }
        // should only be used for mock, obsolete when MQTT is fully implemented
        private static GardenParameters _currentState = new();

        public GardenParameters GetLatestState()
        {
            return _mqttWrapper.GetState();
        }

        public void UpdateState(GardenParameters state)
        {
            _currentState = state;
        }

        public void OpenPump()
        {
            var req = new TurnOnPumpRequest() { TurnOnPump = true };
            var camelSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var reqString = JsonConvert.SerializeObject(req, camelSettings);

            _mqttWrapper.PublishToTopic(Topics.SendingToGarden, reqString);
        }

        public void OpenWindows()
        {
            var req = new TurnOnWindowRequest() { TurnOnWindow = true };
            var camelSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var reqString = JsonConvert.SerializeObject(req, camelSettings);

            _mqttWrapper.PublishToTopic(Topics.SendingToGarden, reqString);
        }
    }
}
