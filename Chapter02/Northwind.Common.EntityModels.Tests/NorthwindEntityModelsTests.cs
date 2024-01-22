using Packt.Shared;

namespace Northwind.Common.EntityModels.Tests
{
    public class NorthwindEntityModelsTests
    {
        [Fact]
        public void CanConnectIsTrue()
        {
            using NorthwindContext context = new();
            bool canConnect = context.Database.CanConnect();
            Assert.True(canConnect);
        }

        [Fact]
        public void ProviderIsSqlServer()
        {
            using NorthwindContext context = new();
            string? provider = context.Database.ProviderName;
            Assert.Equal("Microsoft.EntityFrameworkCore.SqlServer", provider);
        }

        [Fact]
        public void ProductIdIsChai()
        {
            using NorthwindContext context = new();
            Product product = context.Products.Single(p => p.ProductId == 1);
            Assert.Equal("Chai", product.ProductName);
        }

        [Fact]
        public void EmployeeHasLastRefreshedIn10sWindow()
        {
            using NorthwindContext context = new();
            Employee employee = context.Employees.Single(p => p.EmployeeId == 1);
            DateTimeOffset now = DateTimeOffset.UtcNow;

            Assert.InRange(
                actual: employee.LastRefreshed,
                low: now.Subtract(TimeSpan.FromSeconds(5)),
                high: now.AddSeconds(5));
        }
    }
}