using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
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
using System.Threading.Tasks;

namespace Pet.Services.OrderAPI.Messaging
{
    public class AzureServiceBusConsumer: IAzureServiceBusConsumer
    {
        private readonly IOrderRepository _orderrepository;
        private readonly string _subscribtionCheckout;
        private readonly string _connectionstring;
        private readonly string _queueconnectionorder;
        private readonly IConfiguration _configuration;
        private readonly ServiceBusProcessor _checkoutprocessor;
        private readonly IMessageBus _messsageBus;
        private readonly string _queuepayementconnection;
        private readonly string _queueconnectionpayement;
        private readonly ServiceBusProcessor _updatepayementprocess;
     
       
        public AzureServiceBusConsumer(IOrderRepository orderrepository, IConfiguration configuration,IMessageBus MESSAGEBUS)
        {


            _orderrepository = orderrepository;
            _configuration = configuration;
            _subscribtionCheckout = _configuration.GetValue<string>("subscriptiontopic");
            _connectionstring = _configuration.GetValue<string>("ServiceBusConnection");
       
            _queueconnectionpayement = _configuration.GetValue<string>("messagetopic");
            _messsageBus = MESSAGEBUS;
            _queuepayementconnection = _configuration.GetValue<string>("messagepayementqueue");

            var client = new ServiceBusClient(_connectionstring);
            _checkoutprocessor = client.CreateProcessor(_queueconnectionorder);
            _updatepayementprocess = client.CreateProcessor(_queueconnectionpayement);
        }
        public async Task start()
        {

            _checkoutprocessor.ProcessMessageAsync += OnCheckOutMessageReceived;
            _checkoutprocessor.ProcessErrorAsync += errorhandler;
            await _checkoutprocessor.StartProcessingAsync();
            _updatepayementprocess.ProcessMessageAsync += PayementMessageReceived;
            _updatepayementprocess.ProcessErrorAsync += errorhandler;
            await _updatepayementprocess.StartProcessingAsync();
        }
        public async Task stop()
        {

            await _checkoutprocessor.StartProcessingAsync();

            await _checkoutprocessor.DisposeAsync();

            await _updatepayementprocess.StartProcessingAsync();
            await _updatepayementprocess.DisposeAsync();
        }
        private  Task errorhandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
        private async Task OnCheckOutMessageReceived(ProcessMessageEventArgs args)
        {

            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            CheckoutHeaderDto checkoutheader = JsonConvert.DeserializeObject<CheckoutHeaderDto>(body);

            OrderHeader orderheader = new OrderHeader()
            {

                UserId = checkoutheader.UserId,
                FirstName = checkoutheader.FirstName,
                Lastname = checkoutheader.Lastname,
                OrderDetails = new List<OrderDetails>(),
                CartNumber=checkoutheader.CartNumber,
                CouponCode=checkoutheader.CouponCode,
                CVV=checkoutheader.CVV,
                DiscountTotal=checkoutheader.DiscountTotal,
                Email=checkoutheader.Email,
                ExpiryMonthYear=checkoutheader.ExpiryMonthYear,
                OrderTime=DateTime.Now,
                OrderTotal=checkoutheader.OrderTotal,
                paymentstatus=false,
                Phone=checkoutheader.Phone,
                PickupDateTime=checkoutheader.PickupDateTime,
            



            };

            foreach(var details in checkoutheader.CartDetails)
            {


                OrderDetails orderdetails = new OrderDetails
                {
                    ProductId=details.ProductId,
                    ProductName=details.Product.Name,
                    Count=details.Count,
                    Price=details.Product.Price,


                };
                orderheader.CartTotalItems += details.Count;
                orderheader.OrderDetails.Add(orderdetails);
            }



            await _orderrepository.AddOrder(orderheader);
            var payementdetails = new PayementDetails
            {
                OrderId = checkoutheader.CartHeaderId,
                CVV = checkoutheader.CVV,
               CartNumber=checkoutheader.CartNumber,
                ExpiryMonthYear = checkoutheader.ExpiryMonthYear,
                Name=checkoutheader.FirstName+" "+checkoutheader.Lastname,
                OrderTotal = checkoutheader.OrderTotal,
               

            };
            try
            {
                await _messsageBus.PublicMessage(payementdetails, _queuepayementconnection);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            

        }
        private async Task PayementMessageReceived(ProcessMessageEventArgs args)
        {

            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            UpdatePayementResultMessage checkoutheader = JsonConvert.DeserializeObject<UpdatePayementResultMessage>(body);

           

            await _orderrepository.UpdateOrderPayementStatus(checkoutheader.OrderId,checkoutheader.statuspayement);

            await args.CompleteMessageAsync(args.Message);

        }
    }
}
