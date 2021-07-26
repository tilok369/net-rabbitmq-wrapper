# net-rabbitmq-wrapper
 A simple Rabbit MQ wrapper library based on RabbitMQ.Client which provides facilities to initialize Connection, Pubsliher and Subscriber through Dependency Injection .Net Core projects.

# How to use?

# Direct use without DI
```
var mqConnection = new MqConnection("your_connection_url");

 var publisher = new MqPublisher(mqConnection, "your-exchange-name", ExchangeType.Topic);
 var subscriber = new MqSubscriber(mqConnection, "your-exchange-name", "your-queue-name",
     "your-routing-key", ExchangeType.Topic);

 var headers = new Dictionary<string, object> { { "", "" } };

 publisher.Publish("your-routing-key", "This is sample QUEUE message", 
     headers);

 subscriber.Subscribe((message, headers) =>
 {
     Console.WriteLine(message);
     return true;
 });
```
