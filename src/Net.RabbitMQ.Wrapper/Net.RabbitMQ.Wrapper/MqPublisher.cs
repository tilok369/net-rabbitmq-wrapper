using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.RabbitMQ.Wrapper
{
    public class MqPublisher : IMqPublisher
    {
        private bool _disposed;
        private readonly IMqConnection _mqConnection;
        private readonly string _exchange;
        private readonly IModel _model;

        /// <summary>
        /// This constructor initializes Publisher with default Topic exchange and predefined expiration time of 50000
        /// </summary>
        /// <param name="mqConnection"></param>
        /// <param name="exchange"></param>
        public MqPublisher(
            IMqConnection mqConnection,
            string exchange
            ) : this(mqConnection, exchange, ExchangeType.Topic, 50000)
        { }

        /// <summary>
        /// This constructor initializes Publisher with predefined expiration time of 50000
        /// </summary>
        /// <param name="mqConnection"></param>
        /// <param name="exchange"></param>
        /// <param name="exchangeType"></param>
        public MqPublisher(
            IMqConnection mqConnection,
            string exchange,
            string exchangeType
            ) : this(mqConnection, exchange, exchangeType, 50000) 
        { }

        /// <summary>
        /// In this constructor evrything needs to be set
        /// </summary>
        /// <param name="mqConnection"></param>
        /// <param name="exchange"></param>
        /// <param name="exchangeType"></param>
        /// <param name="experation"></param>
        public MqPublisher(
            IMqConnection mqConnection, 
            string exchange, 
            string exchangeType, 
            int experation = 50000
            )
        {
            _mqConnection = mqConnection;
            _exchange = exchange;
            _model = _mqConnection.GetConnection().CreateModel();
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", experation }
            };
            _model.ExchangeDeclare(_exchange, exchangeType, arguments: ttl);
        }

        /// <summary>
        /// Publisher function
        /// </summary>
        /// <param name="routingKey">Routing Key</param>
        /// <param name="message">Publishing message</param>
        /// <param name="headerAttributes">Header attributes</param>
        /// <param name="expirationTime">Expiration time</param>
        public void Publish(string routingKey, string message, IDictionary<string, object> headerAttributes, int expirationTime = 50000)
        {
            var body = Encoding.UTF8.GetBytes(message);
            var properties = _model.CreateBasicProperties();
            properties.Persistent = true;
            properties.Headers = headerAttributes;
            properties.Expiration = expirationTime.ToString();

            _model.BasicPublish(_exchange, routingKey, properties, body);
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
                _model?.Close();

            _disposed = true;
        }
    }
}
