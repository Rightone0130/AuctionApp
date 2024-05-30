using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
namespace RoomService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        #private readonly RabbitMQService _rabbitMQService;

        public RoomController(RabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }

        
        [HttpPost("enter")]
        public IActionResult EnterRoom([FromBody] string User)
        {
            _rabbitMQService.Publish("enter_queue", user);
            return Ok(new { Message = "User entered bidding room", User = user });
        }



        // [HttpPost("enter")]
        // public IActionResult EnterRoom()
        // {
        //     using (var connection = _factory.CreateConnection())
        //     using (var channel = connection.CreateModel())
        //     {
        //         channel.QueueDeclare(queue: "bidding_queue",
        //                              durable: false,
        //                              exclusive: false,
        //                              autoDelete: false,
        //                              arguments: null);

        //         string message = "User entered bidding room";
        //         var body = Encoding.UTF8.GetBytes(message);

        //         channel.BasicPublish(exchange: "",
        //                              routingKey: "bidding_queue",
        //                              basicProperties: null,
        //                              body: body);
        //         return Ok(new { Message = "User entered bidding room" });
        //     }
        // }
    }
}
