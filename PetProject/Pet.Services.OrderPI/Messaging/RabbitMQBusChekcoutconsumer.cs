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
    public class RabbitMQBusChekcoutconsumer : BackgroundService
    {
        private readonly OrderRepository _orderRepository;
        private IConnection _connection;
        private IModel _channel;
        private readonly IConfiguration _configuration;
        private readonly string _queuename;
        private readonly Irabbit _rabbit;
        public RabbitMQBusChekcoutconsumer(OrderRepository orderRepository, IConfiguration configuration, Irabbit rabbit)
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
            _queuename=_configuration.GetValue<string>("QueueName");
            _channel.QueueDeclare(queue: _queuename, false, false, false, arguments: null);
        }
        protected override   Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) => {

                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                CheckoutHeaderDto checkoutheader = JsonConvert.DeserializeObject<CheckoutHeaderDto>(content);
                HandlemMessage(checkoutheader).GetAwaiter().GetResult();
                _channel.BasicAck(ea.DeliveryTag, false);

            };
            _channel.BasicConsume(_queuename, false, consumer);
            return Task.CompletedTask;


        }

        private  async Task HandlemMessage(CheckoutHeaderDto checkoutheader)
        {
            OrderHeader orderheader = new OrderHeader()
            {

                UserId = checkoutheader.UserId,
                FirstName = checkoutheader.FirstName,
                Lastname = checkoutheader.Lastname,
                OrderDetails = new List<OrderDetails>(),
                CartNumber = checkoutheader.CartNumber,
                CouponCode = checkoutheader.CouponCode,
                CVV = checkoutheader.CVV,
                DiscountTotal = checkoutheader.DiscountTotal,
                Email = checkoutheader.Email,
                ExpiryMonthYear = checkoutheader.ExpiryMonthYear,
                OrderTime = DateTime.Now,
                OrderTotal = checkoutheader.OrderTotal,
                paymentstatus = false,
                Phone = checkoutheader.Phone,
                PickupDateTime = checkoutheader.PickupDateTime,




            };

            foreach (var details in checkoutheader.CartDetails)
            {


                OrderDetails orderdetails = new OrderDetails
                {
                    ProductId = details.ProductId,
                    ProductName = details.Product.Name,
                    Count = details.Count,
                    Price = details.Product.Price,


                };
                orderheader.CartTotalItems += details.Count;
                orderheader.OrderDetails.Add(orderdetails);
            }



            await _orderRepository.AddOrder(orderheader);
            var payementdetails = new PayementDetails
            {
                OrderId = orderheader.OrderHeaderId,
                CVV = checkoutheader.CVV,
                CartNumber = checkoutheader.CartNumber,
                ExpiryMonthYear = checkoutheader.ExpiryMonthYear,
                Name = checkoutheader.FirstName + " " + checkoutheader.Lastname,
                OrderTotal = checkoutheader.OrderTotal,
            

            };
            try
            {
                await _rabbit.PublicMessage(payementdetails, "orderpaymentprcesstopic");
                
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }
    }
}
