using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Services.PayementAPI.Messaging
{
    public interface IAzureServiceBusConsumer
    {
        Task start();
        Task stop();
    }
}
