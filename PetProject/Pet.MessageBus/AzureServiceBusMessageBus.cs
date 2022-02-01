using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Pet.MessageBus
{
    public class AzureServiceBusMessageBus : IMessageBus
    {
        private readonly string connection = "Endpoint=sb://petstore.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=sPj4WHQyP7sijeQDv5O18ltiWqOpb4RokZAIQb0OAcI=";
        public async Task PublicMessage(BaseMessage basemessage, string Topic)
        {
            var options = new ServiceBusClientOptions();

            options.TransportType = ServiceBusTransportType.AmqpWebSockets;
         


            await using var client = new ServiceBusClient(connection,options);
      

            ServiceBusSender sender = client.CreateSender(Topic);
            var jsonMessage = JsonConvert.SerializeObject(basemessage);
            var finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };


            await sender.SendMessageAsync(finalMessage);
      
                await client.DisposeAsync();

        }
    }
}
