using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

namespace RoomService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly ConnectionFactory _factory;

        public RoomController()
        {
            _factory = new ConnectionFactory() { HostName = "rabbitmq" };
        }

        [HttpPost("enter")]
        public IActionResult EnterRoom()
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "bidding_queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = "User entered bidding room";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "bidding_queue",
                                     basicProperties: null,
                                     body: body);
                return Ok(new { Message = "User entered bidding room" });
            }
        }
    }
}
