

using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Auto.Notifier
{
    internal class Program
    {
        const string SIGNALR_HUB_URL = "http://localhost:5000/hub";
        private static HubConnection hub;
        
   //     private static readonly IConfigurationRoot config = ReadConfiguration();
        private const string SUBSCRIBER_ID = "Auto.AuditLog";

        static async Task Main(string[] args)
        {
            hub = new HubConnectionBuilder().WithUrl(SIGNALR_HUB_URL).Build();
            await hub.StartAsync();
            Console.WriteLine("Hub started!");
            Console.WriteLine("Press any key to send a message (Ctrl-C to quit)");
            var amqp = "amqp://user:rabbitmq@localhost:5672";
            using var bus = RabbitHutch.CreateBus(amqp);
            Console.WriteLine("Connected to bus! Listening for newOwnerMessages");
            var subscriberId = $"Auto.Notifier";
            await bus.PubSub.SubscribeAsync<NewOwnerVecMessage>(subscriberId, HandleNewOwnerMessage);
            Console.ReadLine();
        }

        private static async Task HandleNewOwnerMessage(NewOwnerVecMessage nvpm)
        {
            var csvRow =
                $"{nvpm.Name} : {nvpm.Address}," +
                $"{nvpm.NumberPhone},{nvpm.RegistrationCode},{nvpm.Color}, {nvpm.Model}{nvpm.ListedAtUtc:O}";
            Console.WriteLine(csvRow);
            var message = JsonConvert.SerializeObject(new NewOwnerVecMessage()
            {
                Name = nvpm.Name,
                Address = nvpm.Address,
                Model = nvpm.Model
            });
           // var json = JsonSerializer.Serialize(nvpm, JsonSettings());
            await hub.SendAsync("NotifyWebUsers", "Auto.Notifier",
                message);
        }

        static JsonSerializerOptions JsonSettings() =>
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        // private static IConfigurationRoot ReadConfiguration()
        // {
        //     var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;
        //     return new ConfigurationBuilder()
        //         .SetBasePath(basePath)
        //         .AddJsonFile("appsettings.json")
        //         .AddEnvironmentVariables()
        //         .Build();
        // }
    }
}