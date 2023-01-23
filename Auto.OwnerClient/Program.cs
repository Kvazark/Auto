using System;
using System.Threading.Tasks;
using Auto.InfoOwnerServer;
using Auto.Messages;
using EasyNetQ;
using Grpc.Net.Client;
//
// using var channel = GrpcChannel.ForAddress("http://localhost:5263");
// var grpcClient = new OwnerInfo.OwnerInfoClient(channel);
// Console.WriteLine("Ready! Press any key to send a gRPCrequest (or Ctrl-C to quit):");
//
// // Console.WriteLine("Enter email:");
// // var email = Console.ReadLine();
// var number = "89141869814";
// var request = new OwnerInfoRequest
// {
//     Number = number
// };
//
// var reply = grpcClient.GetOwnerInfo(request);
// Console.WriteLine($"Information about {reply.Name}: lives at {reply.Address}; vehicle registration number:{reply.Registration}.");
class Program
{
    private static OwnerInfo.OwnerInfoClient grpcClient;
    private static IBus bus;

    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting Auto.OwnerInfoClient");
        var amqp = "amqp://user:rabbitmq@localhost:5672";
        bus = RabbitHutch.CreateBus(amqp);
        Console.WriteLine("Connected to bus; Listening fot newOwnerMessage");
        var grpcAddress = "http://localhost:5263";
        using var channel = GrpcChannel.ForAddress(grpcAddress);
        grpcClient = new OwnerInfo.OwnerInfoClient(channel);
        Console.WriteLine($"Connected to gRPC on {grpcAddress}!");
        var subscriderId = $"Auto.OwnerInfoClient@{Environment.MachineName}";
        await bus.PubSub.SubscribeAsync<NewOwnerMessage>(subscriderId, HandleNewOwnerMessage);
        Console.WriteLine("Press Enter to exit");
        Console.ReadLine();
    }

    private static async Task HandleNewOwnerMessage(NewOwnerMessage message)
    {
        Console.WriteLine($"new owner: {message.Name}");
        var ownerInfoRequest = new OwnerInfoRequest
        {
            Registration = message.RegistrationCode
        };
        var ownerReply = await grpcClient.GetOwnerInfoAsync(ownerInfoRequest); 
        Console.WriteLine($"Information about {message.Name}: owns a {ownerReply.Model} {ownerReply.Color} car. Lives in the {message.Address}.");
        var newOwnerVecMessage = new NewOwnerVecMessage(message, ownerReply.Color, ownerReply.Model);
        await bus.PubSub.PublishAsync(newOwnerVecMessage);
    }
}
