using System;
using Auto.OwnerEngine;
using Grpc.Net.Client;
using var channel = GrpcChannel.ForAddress("http://localhost:5043");
var grpcClient = new Amounter.AmounterClient(channel);
Console.WriteLine("Ready! Press any key to send a gRPC request (or Ctrl-C to quit):");
while (true) {
    Console.ReadKey(true);
    var request = new AmountRequest {
        Name = "Романовa Варвара Дмитриевна"
    };
    var reply = grpcClient.GetAmount(request);
    Console.WriteLine($"{request.Name} had {reply.Amount}");
}