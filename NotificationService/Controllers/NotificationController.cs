using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace NotificationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ConnectionFactory _factory;

        public NotificationController()
        {
            _factory = new ConnectionFactory() { HostName = "rabbitmq" };
        }

        [HttpGet("notify")]
        public IActionResult Notify()
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "notification_queue",
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

                channel.BasicConsume(queue: "notification_queue",
                                     autoAck: true,
                                     consumer: consumer);

                return Ok(new { Message = message });
            }
        }
    }
}
