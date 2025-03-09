using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace ECommerce.EmailAPI.Messaging
{
    //This Service being used to listen on the emails that you queued already and when the Service Bus send it will reflect on this service.

    public class AzureServiceBusConsumer:IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailProductQueue;
        private readonly IConfiguration _configuration;

        private ServiceBusProcessor _emailProductProcessor;

        public AzureServiceBusConsumer(IConfiguration configuration)
        {
            _configuration = configuration;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            emailProductQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailProductQueue");
            
            var client = new ServiceBusClient(serviceBusConnectionString);

            _emailProductProcessor = client.CreateProcessor(emailProductQueue);
        }

        public async Task start()
        {
            _emailProductProcessor.ProcessMessageAsync += OnEmailProductRequestReciver;
            _emailProductProcessor.ProcessMessageAsync += ErrorHandler;

            await _emailProductProcessor.StartProcessingAsync();
        }

        public async Task stop()
        {
            await _emailProductProcessor.StopProcessingAsync();
            await _emailProductProcessor.DisposeAsync();
        }

        private Task ErrorHandler(ProcessMessageEventArgs arg)
        {
            Console.WriteLine("Error Happen HELP");
            return Task.CompletedTask;
        }

        private async Task OnEmailProductRequestReciver(ProcessMessageEventArgs arg)
        {
            var message = arg.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            var objMessage = JsonConvert.DeserializeObject<string>(body);
            try
            {
                //try to log the email
                await arg.CompleteMessageAsync(arg.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
