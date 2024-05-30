using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BiddingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidController : ControllerBase
    {
        private readonly ConnectionFactory _factory;

        public BidController()
        {
            _factory = new ConnectionFactory() { HostName = "rabbitmq" };
        }

        [HttpPost("submit")]
        public IActionResult SubmitBid([FromBody] string bid)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "bidding_queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(bid);

                channel.BasicPublish(exchange: "",
                                     routingKey: "bidding_queue",
                                     basicProperties: null,
                                     body: body);
                return Ok(new { Message = "Bid submitted", Bid = bid });
            }
        }

        [HttpGet("receive")]
        public IActionResult ReceiveBid()
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "bidding_queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                string message = "";

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    message = Encoding.UTF8.GetString(body);
                };

                channel.BasicConsume(queue: "bidding_queue",
                                     autoAck: true,
                                     consumer: consumer);

                return Ok(new { Message = message });
            }
        }
    }
}
