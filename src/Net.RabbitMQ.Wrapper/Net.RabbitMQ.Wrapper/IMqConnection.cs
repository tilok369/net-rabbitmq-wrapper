using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.RabbitMQ.Wrapper
{
    public interface IMqConnection: IDisposable
    {
        IConnection GetConnection();
    }
}
