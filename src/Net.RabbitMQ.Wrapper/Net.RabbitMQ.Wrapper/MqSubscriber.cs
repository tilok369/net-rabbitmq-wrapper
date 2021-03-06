using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.RabbitMQ.Wrapper
{
    public class MqSubscriber : IMqSubscriber
    {
        private bool _disposed;
        private IMqConnection _mqConnection;
        private string _exchange;
        private string _queue;
        private IModel _model;

        /// <summary>
        /// This constructor initializes Subscriber with default Topic exchange and predefined expiration time and prefetch size
        /// </summary>
        /// <param name="mqConnection"></param>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="routingKey"></param>
        public MqSubscriber(
            IMqConnection mqConnection,
            string exchange,
            string queue,
            string routingKey
            ) : this(mqConnection, exchange, queue, routingKey, ExchangeType.Topic, 50000, 10)
        { }

        /// <summary>
        /// This constructor initializes Subscriber with predefined expiration time and prefetch size
        /// </summary>
        /// <param name="mqConnection"></param>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="routingKey"></param>
        /// <param name="exchangeType"></param>
        public MqSubscriber(
            IMqConnection mqConnection,
            string exchange,
            string queue,
            string routingKey,
            string exchangeType
            ) : this(mqConnection, exchange, queue, routingKey, exchangeType, 50000, 10)
        { }

        /// <summary>
        /// This constructor needs everything to be set via params
        /// </summary>
        /// <param name="mqConnection"></param>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="routingKey"></param>
        /// <param name="exchangeType"></param>
        /// <param name="expiration"></param>
        /// <param name="prefetchSize"></param>
        public MqSubscriber(
            IMqConnection mqConnection,
            string exchange,
            string queue,
            string routingKey,
            string exchangeType,
            int expiration = 50000,
            ushort prefetchSize = 10
            )
        {
            _mqConnection = mqConnection;
            _exchange = exchange;
            _queue = queue;
            _model = _mqConnection.GetConnection().CreateModel();
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", expiration }
            };
            _model.ExchangeDeclare(_exchange, exchangeType, arguments: ttl);
            _model.QueueDeclare(_queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _model.QueueBind(_queue, _exchange, routingKey);
            _model.BasicQos(0, prefetchSize, false);
        }

        /// <summary>
        /// Subcribe function with event receiver parameter
        /// </summary>
        /// <param name="receiver">Receiver event Func</param>
        public void Subscribe(Func<string, IDictionary<string, object>, bool> receiver)
        {
            var consumer = new EventingBasicConsumer(_model);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                bool success = receiver.Invoke(message, e.BasicProperties.Headers);
                if (success)
                {
                    _model.BasicAck(e.DeliveryTag, true);
                }
            };

            _model.BasicConsume(_queue, false, consumer);
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
