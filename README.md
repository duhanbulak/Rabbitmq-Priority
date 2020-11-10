
# Prioritized messages on Rabbitmq
Prioritizing messages in queue on Rabbitmq

# Information
RabbitMQ has priority queue implementation in the core as of version 3.5.0

If you want to read the rabbitmq document, [click here!](https://www.rabbitmq.com/priority.html)

# how to prioritize
a. Firstly set environment variable(Don't forget to add for producer and consumer)
```java
   RABBITMQ_URI=amqp://root:root@127.0.0.1:13034
```

b. To declare a priority queue, use the x-max-priority optional queue argument. 
The implementation supports a limited number of priorities: 255. Values between 1 and 10 are recommended.
```java
  channel.QueueDeclare(queue: "PriortyQueue",
       durable: false,
       exclusive: false,
       autoDelete: false,
       arguments: new Dictionary<string, object>()
       {
           { "x-max-priority", 10 },
       });
```
*It is recommended to do both producer and consumer.

c. Create a new IBasicProperties and set message priority

```java
  IBasicProperties props = channel.CreateBasicProperties();
  props.Priority = (byte)priority;
```

d. Finally add BasicProperties when sending the message

```java
  channel.BasicPublish(exchange: "",
       routingKey: "PriortyQueue",
       basicProperties: props,
       body: body);
```

Messages in the queue will now consume in order.
