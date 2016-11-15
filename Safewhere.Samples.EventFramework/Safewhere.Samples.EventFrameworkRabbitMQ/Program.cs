using System;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Safewhere.Samples.EventFrameworkRabbitMQ
{
    class Program
    {
        private static readonly JsonSerializer Serializer = JsonSerializer.Create(new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        static void Main(string[] args)
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings["RabbitMQ"].ConnectionString;
            string topic = ConfigurationManager.AppSettings["Topic"];

            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = connectionString;
            
            IConnection conn = factory.CreateConnection();

            IModel channel = conn.CreateModel();


            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body;
                using (MemoryStream memoryStream = new MemoryStream(body))
                {
                    using (var streamReader = new StreamReader(memoryStream))
                    {
                        using (var textReader = new JsonTextReader(streamReader))
                        {
                            // ... process the message
                            var jsonBody = Serializer.Deserialize(textReader);
                            Console.WriteLine("MessageID: " + ea.BasicProperties.MessageId);
                            Console.WriteLine("CorrelationId: " + ea.BasicProperties.CorrelationId);
                            // This tells you what kind of event that happened on the Identify side
                            Console.WriteLine("MessageType: " + ea.BasicProperties.Type);
                            Console.WriteLine("AppId: " + ea.BasicProperties.AppId);
                            Console.WriteLine("MessageJson: " + jsonBody);
                        }
                    }
                channel.BasicAck(ea.DeliveryTag, false);
                }
            };
            string consumerTag = channel.BasicConsume(topic, false, consumer);

            Console.ReadLine();
        }
    }
}
