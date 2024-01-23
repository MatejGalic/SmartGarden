using Microsoft.AspNetCore.Mvc;
using SmartGarden.Hubs;
using SmartGarden.Models;
using SmartGarden.Services.Garden;

namespace SmartGarden.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GardenController : ControllerBase
    {
        private readonly IGardenService _gardenService;
        private readonly GardenHub _hub;

        public GardenController(IGardenService gardenService, GardenHub hub)
        {
            _gardenService = gardenService;
            _hub = hub;
        }

        [HttpGet]
        public ActionResult<GardenParameters> GetLatestGardenState()
        {
            return Ok(_gardenService.GetLatestState());
        }

        [HttpPut]
        public async Task<ActionResult> UpdateGardenState([FromBody] GardenParameters state)
        {
            _gardenService.UpdateState(state);
            await _hub.BroadcastGardenState(state);
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
