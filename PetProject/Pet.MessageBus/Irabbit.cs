using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pet.MessageBus
{
    public interface Irabbit
    {
        Task PublicMessage(BaseMessage basemessage, string Topic);
    }
}
