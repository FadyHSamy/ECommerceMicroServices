namespace ECommerce.EmailAPI.Messaging
{
    public interface IAzureServiceBusConsumer
    {
        Task start();
        Task stop();
    }
}
