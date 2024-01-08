namespace SmartGarden.Models
{
    public class SolarPanelResults
    {
        /// <summary>
        /// Ukupno proizvedena energija uz podatke za smanjenje (Wh/panel/dan)
        /// </summary>
        public double CumulativeEnergy { get; set; }
        /// <summary>
        /// Ukupno proizvedena energija uz efikasnost (Wh/panel/dan)
        /// </summary>
        public double CumulativeEnergyEfficiency { get; set; }
    }
}
