using SmartGarden.Models;

namespace SmartGarden.Services.Garden
{
    public class GardenService : IGardenService
    {
        private static GardenParameters _currentState = new();

        public GardenParameters GetLatestState()
        {
            return _currentState;
        }

        public void UpdateState(GardenParameters state)
        {
            _currentState = state;
        }

        public GardenParameters OpenPump()
        {
            _currentState.IsPumpOpen = true;
            return _currentState;
        }

        public GardenParameters OpenWindows()
        {
            _currentState.IsWindowOpen = true;
            return _currentState;
        }
    }
}
