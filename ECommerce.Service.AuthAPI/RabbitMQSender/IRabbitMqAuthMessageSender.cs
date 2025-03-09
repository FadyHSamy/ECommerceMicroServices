namespace ECommerce.Service.AuthAPI.RabbitMQSender
{
    public interface IRabbitMqAuthMessageSender
    {
        Task SendMessageAsync(object message, string queueName);
    }
}
