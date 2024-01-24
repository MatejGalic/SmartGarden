namespace SmartGarden.Models
{
    public class GardenParameters
    {
        public int Temperature { get; set; }
        public int Humidity { get; set; }
        public int Moisture { get; set; }
        public bool IsWindowOpen { get; set; }
        public bool IsPumpOpen { get; set; }
        //public bool CanManualyTriggerWindow { get; set; }
        //public bool CanManualyTriggerPump{ get; set; }
    }
}
