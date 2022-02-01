using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Pet.MessageBus;
using Pet.Services.OrderAPI.Messages;
using Pet.Services.OrderAPI.Models;
using Pet.Services.OrderAPI.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pet.Services.OrderAPI.Messaging
{
    public class RabbitMQBusPaymentconsumer : BackgroundService
    {
        private readonly OrderRepository _orderRepository;
        private IConnection _connection;
        private IModel _channel;
        private readonly IConfiguration _configuration;
        private readonly string _queuename;
        private readonly Irabbit _rabbit;
        
        public RabbitMQBusPaymentconsumer(OrderRepository orderRepository, IConfiguration configuration, Irabbit rabbit)
        {
            _orderRepository = orderRepository;
            _rabbit = rabbit;
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _configuration = configuration;
            
            _channel.ExchangeDeclare("PublishsubscribepaymentUpdate_exchange", ExchangeType.Fanout);
            _queuename=_channel.QueueDeclare().QueueName;
            _channel.QueueBind(_queuename, "PublishsubscribepaymentUpdate_exchange", "");
        }
        protected override   Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) => {

                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                UpdatePayementResultMessage updatePayementResultMessage = JsonConvert.DeserializeObject<UpdatePayementResultMessage>(content);
                HandlemMessage(updatePayementResultMessage).GetAwaiter().GetResult();
                _channel.BasicAck(ea.DeliveryTag, false);

            };
            _channel.BasicConsume(_queuename, false, consumer);
            return Task.CompletedTask;


        }

        private  async Task HandlemMessage(UpdatePayementResultMessage updatePayementResultMessage)
        {
            await _orderRepository.UpdateOrderPayementStatus(updatePayementResultMessage.OrderId, updatePayementResultMessage.statuspayement);

            
        }
    }
}
