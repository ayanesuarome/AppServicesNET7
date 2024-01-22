using Microsoft.AspNetCore.Mvc;
using Northwind.Grpc.Client.Mvc.Models;
using System.Xml.Linq;

namespace Northwind.Grpc.Client.Mvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public ProductController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [Route("product/products")]
        public async Task<IActionResult> Index()
        {
            HttpClient client = _clientFactory.CreateClient(
            name: "Northwind.Grpc.Service");
            HttpRequestMessage request = new(
            method: HttpMethod.Get, requestUri: "v1/product");
            HttpResponseMessage response = await client.SendAsync(request);

            IEnumerable<ProductReplyModel>? model = await response.Content
            .ReadFromJsonAsync<IEnumerable<ProductReplyModel>>();

            return View(model);
        }
    }
}
