using Northwind.Grpc.Service.Services;

namespace Northwind.Grpc.Service.Extensions;

public static class MapGrpcServiceExtension
{
    public static void MapNorthwindGrpcService(this WebApplication app)
    {
        app.MapGrpcService<GreeterService>();
        app.MapGrpcService<ShipperService>();
        app.MapGrpcService<ProductService>();
    }
}
