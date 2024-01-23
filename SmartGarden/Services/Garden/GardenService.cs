using SmartGarden.Models;

namespace SmartGarden.Services.Garden
{
    public class GardenService : IGardenService
    {
        private static GardenParameters _currentState = new() { Temperature = 0};

        public GardenParameters GetLatestState()
        {
            _currentState.Temperature += 1;
            return _currentState;
        }

        public void UpdateState(GardenParameters state)
        {
            _currentState = state;
        }

        public GardenParameters TogglePump(ToggleOpeningRequest state)
        {
            _currentState.IsPumpOpen = state.ShouldItemOpen;
            return _currentState;
        }

        public GardenParameters ToggleWindows(ToggleOpeningRequest state)
        {
            _currentState.IsPumpOpen = state.ShouldItemOpen;
            return _currentState;
        }
    }
}
