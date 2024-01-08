using SmartGarden.Models;

namespace SmartGarden.Services.Calculator
{
    public interface ICalculatorService
    {
        SolarPanelResults CalculateEnergyProduction(SolarPanelParameters parameters);
    }
}
