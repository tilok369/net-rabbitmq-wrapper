using Net.RabbitMQ.Wrapper;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace Net.RabbitMQ.Runnable
{
    class Program
    {
        static void Main(string[] args)
        {
            var mqConnection = new MqConnection("amqp://guest:guest@localhost:5672");

            var publisher = new MqPublisher(mqConnection, "tbh-exchange", ExchangeType.Topic);
            var subscriber = new MqSubscriber(mqConnection, "tbh-exchange", "tbh.queue",
                "sample.test", ExchangeType.Topic);

            var headers = new Dictionary<string, object> { { "1", "1" } };

            publisher.Publish("sample.test", "This is sample QUEUE message", 
                headers);

            subscriber.Subscribe((message, headers) =>
            {
                Console.WriteLine(message);
                return true;
            });

            Console.ReadKey(true);
        }
    }
}
