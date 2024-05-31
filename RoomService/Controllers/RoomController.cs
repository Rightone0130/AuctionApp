using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
namespace RoomService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly RabbitMQService _rabbitMQService;

        public RoomController(RabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }

        
        [HttpPost("enter")]
        public IActionResult EnterRoom()
        {
            _rabbitMQService.Publish("room_queue", "User entered bidding room" );
             return Ok(new { Message = "User entered bidding room" });
        }

    }
}