using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.ClientFactory;
using Microsoft.AspNetCore.Mvc;
using Northwind.Grpc.Client.Mvc.Models;
using System.Diagnostics;

namespace Northwind.Grpc.Client.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        protected readonly Greeter.GreeterClient greeterClient;
        protected readonly Shipper.ShipperClient shipperClient;

        public HomeController(ILogger<HomeController> logger,
            GrpcClientFactory factory)
        {
            _logger = logger;
            
            // Another way to create instance of gRPC clients
            //GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5121");
            //greeterClient = new Greeter.GreeterClient(channel);
            
            greeterClient = factory.CreateClient<Greeter.GreeterClient>("Greeter");
            shipperClient = factory.CreateClient<Shipper.ShipperClient>("Shipper");
        }

        public async Task<IActionResult> Index(string name = "Ayane", int id = 1)
        {
            try
            {
                HelloReply helloReply = await greeterClient.SayHelloAsync(
                    new HelloRequest { Name = name });

                ViewData["greeting"] = $"Greeting from gRPC service: {helloReply.Message}";

                // without AsyncUnaryCall
                //ShipperReply shipperReply = await shipperClient.GetShipperAsync(
                //    new ShipperRequest
                //    {
                //        ShipperId = id
                //    });

                // with AsyncUnaryCall
                AsyncUnaryCall<ShipperReply> shipperCall = shipperClient.GetShipperAsync(
                    new ShipperRequest {
                        ShipperId = id
                    },
                    deadline: DateTime.UtcNow.AddSeconds(30)
                    );

                Metadata metadata = await shipperCall.ResponseHeadersAsync;

                foreach (Metadata.Entry entry in metadata)
                {
                    _logger.LogCritical($"Key: {entry.Key}, Value: ${entry.Value}");
                }

                ShipperReply shipperReply = await shipperCall.ResponseAsync;

                ViewData["shipper"] = $$"""
                                      Shipper from gRPC service: ID: {{shipperReply.ShipperId}}, Name: {{shipperReply.CompanyName}}, 
                                      Phone: {{shipperReply.Phone}}.
                                      """;
            }
            catch (RpcException rpcex) when (rpcex.StatusCode == global::Grpc.Core.StatusCode.DeadlineExceeded)
            {
                _logger.LogWarning("Northwind.Grpc.Service deadline exceeded.");
                ViewData["Exception"] = rpcex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Northwind.Grpc.Service is not responding.");
                ViewData["Exception"] = ex.Message;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}