using Microsoft.AspNetCore.Mvc;

namespace Cards.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        /// <summary>
        /// Get card by name
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get([FromQuery] string name)
        {
            return Ok($"It's working. I found the card {name} in the database");
        }
    }
}