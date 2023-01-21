using System;
using Auto.InfoOwnerServer;
using Grpc.Net.Client;

using var channel = GrpcChannel.ForAddress("http://localhost:5263");
var grpcClient = new OwnerInfo.OwnerInfoClient(channel);
Console.WriteLine("Ready! Press any key to send a gRPCrequest (or Ctrl-C to quit):");

// Console.WriteLine("Enter email:");
// var email = Console.ReadLine();
var number = "89141869814";
var request = new OwnerInfoRequest
{
    Number = number
};

var reply = grpcClient.GetOwnerInfo(request);
Console.WriteLine($"Information about {reply.Name}: lives at {reply.Address}; vehicle registration number:{reply.Registration}.");