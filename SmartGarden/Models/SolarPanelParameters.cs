namespace SmartGarden.Models
{
    public class SolarPanelParameters
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public double Latitude { get; set; }
        public double PanelTilt { get; set; }
        public double PanelPower { get; set; }
        public double NOCT { get; set; }
        public double AmbientTemperature { get; set; }
        public double PanelEfficiency { get; set; }
        public double PanelArea { get; set; }
        public double PowerTemperatureCoefficient { get; set; }
    }
}