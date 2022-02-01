using Microsoft.AspNetCore.Builder;
using Pet.Services.PayementAPI.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Pet.Services.PayementAPI.Extensions
{
    public static  class ApplicationbuilderExtensions
    {
        public static IAzureServiceBusConsumer ServiceBusConsumer { get; set; }
        public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            ServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
            var hostApllicationlife = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            hostApllicationlife.ApplicationStarted.Register(onstart);
            hostApllicationlife.ApplicationStopped.Register(onstop);
            return app;

        }

        static private void onstart()
        {

           ServiceBusConsumer.start();
        }
        static private void onstop()
        {

           ServiceBusConsumer.stop();
        }
    }
    }
