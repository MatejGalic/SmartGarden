using SmartGarden.Models;

namespace SmartGarden.Services.Garden
{
    public interface IGardenService
    {
        GardenParameters GetLatestState();
        void UpdateState(GardenParameters state);
        GardenParameters TogglePump(ToggleOpeningRequest state);
        GardenParameters ToggleWindows(ToggleOpeningRequest state);        
    }
}
