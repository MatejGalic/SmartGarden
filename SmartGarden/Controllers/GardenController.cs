using Microsoft.AspNetCore.Mvc;
using SmartGarden.Models;
using SmartGarden.Services.Garden;

namespace SmartGarden.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GardenController : ControllerBase
    {
        private readonly IGardenService _gardenService;

        public GardenController(IGardenService gardenService)
        {
            _gardenService = gardenService;
        }

        [HttpGet]
        public ActionResult<GardenParameters> GetLatestGardenState()
        {
            return Ok(_gardenService.GetLatestState());
        }

        [HttpPut]
        public ActionResult UpdateGardenState([FromBody] GardenParameters state)
        {
            _gardenService.UpdateState(state);
            return Ok();
        }

        [HttpPost("toggle-pump")]
        public ActionResult<GardenParameters> TogglePump([FromBody] ToggleOpeningRequest state)
        {

            return Ok(_gardenService.TogglePump(state));
        }

        [HttpPost("toggle-windows")]
        public ActionResult<GardenParameters> ToggleWindows([FromBody] ToggleOpeningRequest state)
        {
            return Ok(_gardenService.ToggleWindows(state));
        }
    }
}
