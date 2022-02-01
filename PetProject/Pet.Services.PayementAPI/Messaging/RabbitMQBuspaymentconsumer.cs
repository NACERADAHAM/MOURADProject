using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PaymentProcessor;
using Pet.MessageBus;
using Pet.Services.PayementAPI.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pet.Services.PayementAPI.Messaging
{
    public class RabbitMQBuspaymentconsumer : BackgroundService
    {
      
        private IConnection _connection;
        private IModel _channel;
        private readonly IConfiguration _configuration;
        private readonly string _queuename;
        private readonly string _exchangename;
        private readonly Irabbit _rabbitsender;
        private readonly IProcessPayment _processpayment;
        public RabbitMQBuspaymentconsumer( IConfiguration configuration, Irabbit rabbit, IProcessPayment processpayment)
        {
            _processpayment = processpayment;
            _rabbitsender = rabbit;
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _configuration = configuration;
            _queuename=_configuration.GetValue<string>("QueueName");
            _channel.QueueDeclare(queue: "orderpaymentprcesstopic", false, false, false, arguments: null);
            _exchangename= _configuration.GetValue<string>("ExchangeName");
        }
        protected override   Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) => {

                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                PayementDetails payementdetails = JsonConvert.DeserializeObject<PayementDetails>(content);
                HandlemMessage(payementdetails).GetAwaiter().GetResult();
                _channel.BasicAck(ea.DeliveryTag, false);

            };
            _channel.BasicConsume(_queuename, false, consumer);
            return Task.CompletedTask;


        }

        private  async Task HandlemMessage(PayementDetails payementdetails)
        {
            var Result = _processpayment.payementProcess();
            var updatepayment = new UpdatePayementResultMessage()
            {

                statuspayement = Result,
                OrderId = payementdetails.OrderId,

            };
            try
            {
                await _rabbitsender.PublicMessage(updatepayment, "");
             
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }
    }
}
