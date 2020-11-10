using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
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
                    channel.QueueDeclare(queue: "PriortyQueue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: new Dictionary<string, object>()
                        {
                            {"x-max-priority", 10},
                        });

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += Consumer_Received;

                    channel.BasicConsume(queue: "PriortyQueue",
                        autoAck: true,
                        consumer: consumer);

                    Console.ReadLine();
                }
            }
        }

        static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            Console.WriteLine($" [x] Received {Encoding.UTF8.GetString(e.Body.ToArray())}");
        }
    }
}
