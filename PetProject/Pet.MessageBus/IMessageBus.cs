using System;
using System.Threading.Tasks;

namespace Pet.MessageBus
{
    public interface IMessageBus
    {
        Task PublicMessage(BaseMessage basemessage,string Topic);
    }
}
