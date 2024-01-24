using SmartGarden.Models;

namespace SmartGarden.Services.Garden
{
    public interface IGardenService
    {
        GardenParameters GetLatestState();
        void UpdateState(GardenParameters state);
        void OpenPump();
        void OpenWindows();
    }
}
