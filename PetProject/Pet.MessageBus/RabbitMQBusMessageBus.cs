using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
namespace Pet.MessageBus
{
    public class RabbitMQBusMessageBus : Irabbit
    {
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _username;
        private  IConnection _connection;
        public RabbitMQBusMessageBus()
        {
            _hostname = "localhost";
            _password = "guest";
            _username = "guest";

           
        }
        public async  Task PublicMessage(BaseMessage basemessage, string queuename)
        {
            if (Connectionexist())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queuename, false, false, false, arguments: null);
                    var jsonMessage = JsonConvert.SerializeObject(basemessage);

                    var body = Encoding.UTF8.GetBytes(jsonMessage);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;


                    channel.BasicPublish(exchange: "",
                                        routingKey: queuename,
                                        basicProperties: null,
                                        body: body);

                }
            }
        }

        private bool Connectionexist()
        {
            if (_connection != null)
            {
                return true;
            }else
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _hostname,
                    Password = _password,
                    UserName = _username
                };
                _connection = factory.CreateConnection();
                return true;
            }

        }
    }
}
