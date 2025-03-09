using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace ECommerce.MessageBus
{
    //Using Service to sending a messages 
    public class MessageBus : IMessageBus
    {
        private string connectionString = "PrimaryConnectionString";
        public async Task PublishMessage(object message, string topicQueueName)
        {
            await using var client = new ServiceBusClient(connectionString);

            var sender = client.CreateSender(topicQueueName);

            var jsonMessage = JsonConvert.SerializeObject(message);

            var finalMessage = new ServiceBusMessage(Encoding
                .UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            await sender.SendMessageAsync(finalMessage);
        }
    }
}
