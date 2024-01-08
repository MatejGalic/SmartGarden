using Microsoft.AspNetCore.Mvc;
using SmartGarden.Models;
using SmartGarden.Services.Calculator;

namespace SmartGarden.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculatorService _calculatorService;

        public CalculatorController(ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        [HttpPost]
        public ActionResult<SolarPanelResults> CalculateSolarPanelEnergyProduction([FromBody] SolarPanelParameters parameters)
        {
            //parameters = new SolarPanelParameters
            //{
            //    Year = 2023,
            //    Month = 5,
            //    Day = 15,
            //    Latitude = 45.5,
            //    PanelTilt = 35.0,
            //    PanelPower = 15,
            //    NOCT = 47.0,
            //    AmbientTemperature = 15.0,
            //    PanelEfficiency = 11.9,
            //    PanelArea = 0.1575,
            //    PowerTemperatureCoefficient = -0.5
            //};

            //Ukupno proizvedena energija uz podatke za smanjenje (Wh/panel/dan): 108.083065
            //Ukupno proizvedena energija uz efikasnost(Wh / panel / dan): 146.908868

            return Ok(_calculatorService.CalculateEnergyProduction(parameters));
        }
    }
}
