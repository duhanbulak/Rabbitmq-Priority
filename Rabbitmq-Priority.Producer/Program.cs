using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { Uri = new Uri(Environment.GetEnvironmentVariable("RABBITMQ_URI")) };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    Random r = new Random();

                    channel.QueueDeclare(queue: "PriortyQueue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: new Dictionary<string, object>()
                        {
                            { "x-max-priority", 10 },
                        });

                    IBasicProperties props = channel.CreateBasicProperties();

                    for (int i = 0; i < 30; i++)
                    {
                        int priority = r.Next(10);
                        props.Priority = (byte)priority;

                        string message = $"Hello World! {i} / Priorty: {priority}";
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "",
                            routingKey: "PriortyQueue",
                            basicProperties: props,
                            body: body);

                        Console.WriteLine($" [x] Sent {message}");
                    }
                }

                Console.ReadLine();
            }
        }
    }
}
