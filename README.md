# net-rabbitmq-wrapper
 A simple Rabbit MQ wrapper library based on RabbitMQ.Client which provides facilities to initialize Connection, Pubsliher and Subscriber through Dependency Injection .Net Core projects.

# How to use?

Direct use without DI
----------------------
```C#
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

With Dependency Injection in .NET Core/.NET 5
---------------------------------------------
```C#
services.AddSingleton<IMqConnection>(new MqConnection("your_connection_url"));

services.AddSingleton<IMqPublisher>(x => new MqPublisher(x.GetService<IMqConnection>(),
   "you_exchange_name", ExchangeType.Topic));
   
services.AddSingleton<IMqSubscriber>(x => new MqSubscriber(x.GetService<IMqConnection>(),
    "your_exchange_name", "your_queue_name", "your_routing_key", ExchangeType.Topic));
```

Publish
--------
```C#
_publisher.Publish("loan_account.created", JsonConvert.SerializeObject("your_object"));
```

Subscribe
---------
```C#
_subscriber.Subscribe(Your_Subscription_Event_Method());
```

# Installation-Nuget

<span style="color:blue">Install-Package Net.RabbitMQ.Wrapper -Version 1.0.0</span>

# Contribution
Any kind of contribution is welcomed!
