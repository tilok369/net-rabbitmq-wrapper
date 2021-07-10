using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.RabbitMQ.Wrapper
{
    public interface IMqPublisher: IDisposable
    {
        void Publish(string routingKey, string message, IDictionary<string, object> headerAttributes, int expirationTime);
    }
}
