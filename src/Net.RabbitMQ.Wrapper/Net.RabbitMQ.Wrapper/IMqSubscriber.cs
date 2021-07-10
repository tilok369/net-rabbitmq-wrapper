using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.RabbitMQ.Wrapper
{
    public interface IMqSubscriber : IDisposable
    {
        void Subscribe(Func<string, IDictionary<string, object>, bool> receiver);
    }
}
