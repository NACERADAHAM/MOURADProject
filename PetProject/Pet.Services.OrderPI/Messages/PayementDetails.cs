using Pet.MessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Services.OrderAPI.Messages
{
    public class PayementDetails:BaseMessage
    {
        public int OrderId { get; set; }
       
        public string Name { get; set; }
       

        public string CartNumber { get; set; }
        public string CVV { get; set; }
        public string ExpiryMonthYear { get; set; }
        public Double OrderTotal { get; set; }
    }
}
