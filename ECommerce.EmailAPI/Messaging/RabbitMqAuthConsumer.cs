using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ECommerce.EmailAPI.Messaging
{
    public class RabbitMqAuthConsumer : BackgroundService
    {

        private readonly string _queueName = "RegisterUserQueue";
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IChannel _channel;

        public RabbitMqAuthConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.QueueDeclareAsync(queue: _queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    var email = JsonConvert.DeserializeObject<string>(message);
                    await HandleMessage(email);
                    await _channel.BasicAckAsync(ea.DeliveryTag, false,cancellationToken:stoppingToken); // Acknowledge message
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, true, cancellationToken: stoppingToken); // Requeue message
                }
            };

            await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
            await Task.Delay(Timeout.Infinite, stoppingToken); // Keep consumer running
        }
        private async Task HandleMessage(string email)
        {
            // Simulate email processing
            await Task.Delay(1000);
            Console.WriteLine($"Email sent to: {email}");
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel is not null)
                await _channel.CloseAsync(cancellationToken: cancellationToken);

            if (_connection is not null)
                await _connection.CloseAsync(cancellationToken: cancellationToken);

            _channel?.Dispose();
            _connection?.Dispose();

            await base.StopAsync(cancellationToken);
        }
    }
}
