using Microsoft.AspNetCore.Mvc;

namespace BiddingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidController : ControllerBase
    {
        private readonly RabbitMQService _rabbitMQService;

        public BidController(RabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }

        [HttpPost("submit")]
        public IActionResult SubmitBid([FromBody] string bid)
        {
            _rabbitMQService.Publish("bidding_queue", bid);
            return Ok(new { Message = "Bid submitted", Bid = bid });
        }

        [HttpGet("receive")]
        public IActionResult ReceiveBid()
        {
            var message = _rabbitMQService.Consume("bidding_queue");
            return Ok(new { Message = message });
        }
    }
}






































// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Options;
// using RabbitMQ.Client;
// using RabbitMQ.Client.Events;
// using RabbitMQ.Client.Exceptions;
// using System.Text;

// namespace BiddingService.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class BidController : ControllerBase
//     {
//         private readonly ConnectionFactory _factory;

//         public BidController(IOptions<RabbitMQSettings> rabbitMQOptions)
//         {
//             var settings = rabbitMQOptions.Value;
//             _factory = new ConnectionFactory
//             {
//                 HostName = settings.HostName,
//                 UserName = settings.UserName,
//                 Password = settings.Password
//             };
//         }


//         [HttpPost("submit")]
//         public IActionResult SubmitBid([FromBody] string bid)
//         {
//             SubmitBid submitBid = new(){

//                 Bid = bid

//             };
//             using (var connection = _factory.CreateConnection())
//             using (var channel = connection.CreateModel())
//             {
//                 channel.QueueDeclare(queue: "bidding_queue",
//                                      durable: false,
//                                      exclusive: false,
//                                      autoDelete: false,
//                                      arguments: null);

//                 var body = Encoding.UTF8.GetBytes(submitBid.Bid);

//                 channel.BasicPublish(exchange: "",
//                                      routingKey: "bidding_queue",
//                                      basicProperties: null,
//                                      body: body);
//                 return Ok(new { Message = "Bid submitted", Bid = submitBid.Bid });
//             }
//         }

//         [HttpGet("receive")]
//         public IActionResult ReceiveBid()
//         {
//             using (var connection = _factory.CreateConnection())
//             using (var channel = connection.CreateModel())
//             {
//                 channel.QueueDeclare(queue: "bidding_queue",
//                                      durable: false,
//                                      exclusive: false,
//                                      autoDelete: false,
//                                      arguments: null);

//                 var consumer = new EventingBasicConsumer(channel);
//                 string message = "";

//                 consumer.Received += (model, ea) =>
//                 {
//                     var body = ea.Body.ToArray();
//                     message = Encoding.UTF8.GetString(body);
//                 };

//                 channel.BasicConsume(queue: "bidding_queue",
//                                      autoAck: true,
//                                      consumer: consumer);

//                 return Ok(new { Message = message });
//             }
//         }
//     }

//     public class SubmitBid
//     {
//         public string Bid { get; set; } = string.Empty;

       
//     }
// }
