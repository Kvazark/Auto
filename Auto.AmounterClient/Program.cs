using System;
using Auto.AmounterEngine;
using Grpc.Net.Client;
using var channel = GrpcChannel.ForAddress("http://localhost:5094");
var grpcClient = new Amounter.AmounterClient(channel);
Console.WriteLine("Ready! Press any key to send a gRPC request (or Ctrl-C to quit):");
while (true) {
    // Console.ReadKey(true);
    var fullName = Console.ReadLine();
    string[] address = new string[6] { "Рязанской области", "Московской области", "Курской области", "Лениградской области","Кировской области","Тамбовской области" };
    Random rd = new Random();
    int num = rd.Next(0, 5);
    
    var request = new AmountRequest {
        Name = fullName,
        Address = address[num]
        // Name = "Боброва Валентина Аркадьевна",
        // Address = "Рязанская область, город Кашира_пл, Ломоносова_33"
    };
    var reply = grpcClient.GetAmount(request);
    Console.WriteLine($"{request.Name} живёт в {request.Address}. Было выложено объявлений: {reply.Amount}.");
}