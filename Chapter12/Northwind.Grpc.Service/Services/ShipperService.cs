using Grpc.Core;
using Northwind.Grpc.Service;
using ShipperEntity = Packt.Shared.Shipper;

namespace Northwind.Grpc.Service.Services;

public class ShipperService : Shipper.ShipperBase
{
    protected readonly ILogger<GreeterService> _logger;
    protected readonly Packt.Shared.NorthwindContext _db;

    public ShipperService(ILogger<GreeterService> logger, Packt.Shared.NorthwindContext db)
    {
        _logger = logger;
        _db = db;
    }

    public override async Task<ShipperReply?> GetShipper(ShipperRequest request, ServerCallContext context)
    {
        _logger.LogCritical("This request has a deadline of {0:T}. It is now {1:T}", context.Deadline, DateTime.UtcNow);
        await Task.Delay(TimeSpan.FromSeconds(5));

        ShipperEntity? shipper = await _db.Shippers.FindAsync(request.ShipperId, context.CancellationToken);

        return shipper == null ? null : ToShipperReply(shipper);
    }

    // use AutoMapper
    private ShipperReply ToShipperReply(ShipperEntity shipper)
    {
        return new ShipperReply
        {
            ShipperId = shipper.ShipperId,
            CompanyName = shipper.CompanyName,
            Phone = shipper.Phone
        };
    }
}