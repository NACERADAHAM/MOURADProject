using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Services.OrderAPI.Messaging
{
    public interface IAzureServiceBusConsumer
    {
        Task start();
        Task stop();
    }
}
