using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.RabbitMQ.Wrapper
{
    public class MqConnection : IMqConnection
    {
        private readonly IConnection _connection;
        private readonly IConnectionFactory _connectionFactory;
        private bool _disposed;

        public MqConnection(string mqUrl)
        {
            _connectionFactory = new ConnectionFactory { Uri = new Uri(mqUrl) };
            _connection = _connectionFactory.CreateConnection();
        }

        public IConnection GetConnection()
        {
            return _connection;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _connection?.Close();

            _disposed = true;
        }
    }
}
