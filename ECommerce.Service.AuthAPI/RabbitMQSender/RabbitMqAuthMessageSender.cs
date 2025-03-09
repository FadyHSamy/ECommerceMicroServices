using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace ECommerce.Service.AuthAPI.RabbitMQSender
{
    public class RabbitMqAuthMessageSender : IRabbitMqAuthMessageSender
    {

        private readonly string _hostName;
        private readonly string _userName;
        private readonly string _password;
        private IConnection _connection;

        public RabbitMqAuthMessageSender()
        {
            _hostName = "localhost";
            _userName = "guest";
            _password = "guest";
        }

        public async Task SendMessageAsync(object message, string queueName)
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password,
            };

            // Create a connection (should be reused if possible)
            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            // Ensure the queue exists
            await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: body);

        }

    }
}