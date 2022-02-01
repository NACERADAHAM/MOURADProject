using Pet.MessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Services.PayementAPI.Messages
{
    public class UpdatePayementResultMessage:BaseMessage { 

        public bool statuspayement { get; set; }
        public int OrderId { get; set; }
    }
}
