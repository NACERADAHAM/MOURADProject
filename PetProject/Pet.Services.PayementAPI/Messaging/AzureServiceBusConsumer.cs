using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PaymentProcessor;
using Pet.MessageBus;


using Pet.Services.PayementAPI.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pet.Services.PayementAPI.Messaging
{
    public class AzureServiceBusConsumer: IAzureServiceBusConsumer
    {
     
        private readonly string _subscribtionCheckout;
        private readonly string _connectionstring;
        private readonly string _queueconnection;
        private readonly IConfiguration _configuration;
        private readonly ServiceBusProcessor _checkoutprocessor;
        private readonly IMessageBus _messsageBus;
        private readonly string _queuepayementconnection;
        private readonly IProcessPayment _processpayment;
        public AzureServiceBusConsumer( IConfiguration configuration,IMessageBus MESSAGEBUS, IProcessPayment processpayment)
        {


            _processpayment = processpayment;
            _configuration = configuration;
            _subscribtionCheckout = _configuration.GetValue<string>("subscriptiontopic");
            _connectionstring = _configuration.GetValue<string>("ServiceBusConnection");
            _queueconnection = _configuration.GetValue<string>("messagetopic");
            _messsageBus = MESSAGEBUS;
            _queuepayementconnection = _configuration.GetValue<string>("messagepayementqueue");
            var client = new ServiceBusClient(_connectionstring);
            _checkoutprocessor = client.CreateProcessor(_queueconnection);

        }
        public async Task start()
        {

            _checkoutprocessor.ProcessMessageAsync += OnCheckOutMessageReceived;
            _checkoutprocessor.ProcessErrorAsync += errorhandler;
            await _checkoutprocessor.StartProcessingAsync();
        }
        public async Task stop()
        {

            await _checkoutprocessor.StartProcessingAsync();
            await _checkoutprocessor.DisposeAsync();
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
            PayementDetails payementdetails = JsonConvert.DeserializeObject<PayementDetails>(body);
           var Result= _processpayment.payementProcess();
            var updatepayment = new UpdatePayementResultMessage()
            {
            
            statuspayement=Result,
            OrderId=payementdetails.OrderId,
            
            };



            try
            {
                await _messsageBus.PublicMessage(updatepayment, _queuepayementconnection);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            

        }
        
    }
}
