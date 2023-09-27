// See https://aka.ms/new-console-template for more information
using Grpc.Net.Client;
using GrpcClient;
using RestSharp;

Console.WriteLine("Hello, World!");

var chanel = GrpcChannel.ForAddress(" https://localhost:7138");
var client1 = new Greeter.GreeterClient(chanel);
var result1 = client1.SayHello(new HelloRequest() { Name = "Hello" });
Console.WriteLine(result1);

var client = new Custom.CustomClient(chanel);
var result = client.GetInfo(new GetInfoRequest() { Id = 5 });

Console.WriteLine(result);

var url = "https://localhost:7138/v1/protobuf/5";
var clinet = new RestClient();
var request = new RestRequest(url, Method.Get);
var response = clinet.Execute(request);

Console.WriteLine(response.Content);
