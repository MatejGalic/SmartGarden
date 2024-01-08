namespace SmartGarden.Models
{
    public class SolarPanelParameters
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        /// <summary>
        /// Kut zemljopisne širine (°)
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// Kut nagiba panela (°)
        /// </summary>
        public double PanelTilt { get; set; }
        /// <summary>
        /// Nominalna snaga panela (W)
        /// </summary>
        public double PanelPower { get; set; }
        /// <summary>
        /// Nominalna radna temperatura ćelije (NOCT) (°C)
        /// </summary>
        public double NOCT { get; set; }
        /// <summary>
        /// Ambijentalna temperatura (°C)
        /// </summary>
        public double AmbientTemperature { get; set; }
        /// <summary>
        /// Efikasnost panela (%)
        /// </summary>
        public double PanelEfficiency { get; set; }
        /// <summary>
        /// Površina panela (m2)
        /// </summary>
        public double PanelArea { get; set; }
        /// <summary>
        /// Temperaturni koeficijent snage (%/°C)
        /// </summary>
        public double PowerTemperatureCoefficient { get; set; }
    }
}