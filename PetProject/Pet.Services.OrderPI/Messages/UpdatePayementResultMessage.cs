using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Services.OrderAPI.Messages
{
    public class UpdatePayementResultMessage { 

        public bool statuspayement { get; set; }
        public int OrderId { get; set; }
    }
}
