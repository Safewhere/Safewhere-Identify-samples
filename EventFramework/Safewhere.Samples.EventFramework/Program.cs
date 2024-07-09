using System;
using System.Configuration;
using System.IO;
using System.Text;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Safewhere.Samples.EventFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            string topic = ConfigurationManager.AppSettings["Topic"];

            // here we create a subscription to all messages of a topic which is created for a specific Identify tenant
            var namespaceManager =
                NamespaceManager.CreateFromConnectionString(connectionString);

            if (!namespaceManager.SubscriptionExists(topic, "AllMessages"))
            {
                namespaceManager.CreateSubscription(topic, "AllMessages");
            }

            // create a client for the subcription above to receive message
            SubscriptionClient client =
                SubscriptionClient.CreateFromConnectionString(connectionString, topic, "AllMessages");

            // Configure the callback options.
            OnMessageOptions options = new OnMessageOptions
            {
                AutoComplete = false,
                AutoRenewTimeout = TimeSpan.FromMinutes(1)
            };

            client.OnMessage((message) =>
            {
                try
                {
                    // Process message from subscription.
                    Console.WriteLine("\n**Message**");
                    var stream = message.GetBody<Stream>();
                    stream.Seek(0, SeekOrigin.Begin);
                    var messageJson = new StreamReader(stream, Encoding.UTF8).ReadToEnd();

                    Console.WriteLine("MessageID: " + message.MessageId);
                    Console.WriteLine("CorrelationId: " +  message.CorrelationId);
                    // This tells you what kind of event that happened on the Identify side
                    Console.WriteLine("MessageType: " + message.Properties["MessageType"]);
                    Console.WriteLine("AppId: " + message.Properties["AppId"]);
                    Console.WriteLine("MessageJson: " + messageJson);
                    // Remove message from subscription.
                    message.Complete();
                }
                // For demonstration purpose only. You know that you should never do generic code, right? :)
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    // Indicates a problem, unlock message in subscription.
                    message.Abandon();
                }
            }, options);

            Console.ReadLine();
        }
    }
}
