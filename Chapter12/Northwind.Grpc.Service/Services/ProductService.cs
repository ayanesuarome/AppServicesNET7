using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Northwind.Grpc.Service.Protos;

namespace Northwind.Grpc.Service.Services
{
    public class ProductService : Product.ProductBase
    {
        protected readonly ILogger<GreeterService> _logger;
        protected readonly Packt.Shared.NorthwindContext _db;

        public ProductService(ILogger<GreeterService> logger, Packt.Shared.NorthwindContext db)
        {
            _logger = logger;
            _db = db;
        }

        public override async Task<ProductsReply> GetAlphabeticalListOfProducts(Empty request, ServerCallContext context)
        {
            List<ProductReply> products = _db.AlphabeticalListOfProducts.Select(p => new ProductReply
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                CategoryName = p.CategoryName,
                Discontinued = p.Discontinued
            }).ToList();

            ProductsReply productsReply = new();
            productsReply.Product.AddRange(products);

            return productsReply;
        }
    }
}
