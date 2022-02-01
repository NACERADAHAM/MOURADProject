using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pet.MessageBus
{
    public class BaseMessage
    {
        public string Id { get; set; }
        public DateTime MessageCreated { get; set; }
    }
}
