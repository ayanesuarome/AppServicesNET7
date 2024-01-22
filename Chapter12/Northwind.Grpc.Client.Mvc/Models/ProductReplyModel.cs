namespace Northwind.Grpc.Client.Mvc.Models
{
    public class ProductReplyModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public bool Discontinued { get; set; }
    }
}
