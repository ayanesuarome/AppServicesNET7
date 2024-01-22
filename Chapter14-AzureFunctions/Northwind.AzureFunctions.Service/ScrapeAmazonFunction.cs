using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.AzureFunctions.Service
{
    // Get information about a function: http://localhost:7033/admin/functions/{function-name}
    // [POST] http://localhost:7033/admin/functions/ScrapeAmazonFunction
    public class ScrapeAmazonFunction
    {
        private const string relativePath = "10-NET-Cross-Platform-Development-websites/dp/1801077363/";
        private readonly IHttpClientFactory _clientFactory;

        public ScrapeAmazonFunction(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        // Every hour
        [FunctionName(nameof(ScrapeAmazonFunction))]
        public async Task Run([TimerTrigger("0 0 * * * *")] TimerInfo timer, ILogger log)
        {
            log.LogInformation($"Timer trigger function executed at: {DateTime.UtcNow}");
            log.LogInformation($"Timer trigger function next three occurrences at: {timer.FormatNextOccurrences(3, DateTime.UtcNow)}.");

            HttpClient client = _clientFactory.CreateClient("Amazon");
            HttpResponseMessage response = await client.GetAsync(relativePath);

            log.LogInformation($"Request: GET {client.BaseAddress}{relativePath}");

            if(response.IsSuccessStatusCode)
            {
                log.LogInformation($"Successful HTTP request.");

                // read the content from a GZIP stream into a string
                Stream stream = await response.Content.ReadAsStreamAsync();
                GZipStream gzipStream = new(stream, CompressionMode.Decompress);
                StreamReader reader = new(gzipStream);
                string page = reader.ReadToEnd();

                // extract the Best Sellers Rank
                int posBsr = page.IndexOf("Best Sellers Rank");
                string bsrSection = page.Substring(posBsr, 45);

                // bsrSection will be something like:
                // "Best Sellers Rank: </span> #22,258 in Books ("
                // get the position of the # and the following space
                int posHash = bsrSection.IndexOf("#") + 1;
                int posSpaceAfterHash = bsrSection.IndexOf(" ", posHash);

                // get the BSR number as text
                string bsr = bsrSection.Substring(posHash, posSpaceAfterHash - posHash);
                bsr = bsr.Replace(",", null); // remove commas

                // parse the text into a number
                if (int.TryParse(bsr, out int bestSellersRank))
                {
                    log.LogInformation($"Best Sellers Rank #{bestSellersRank:N0}.");
                }
                else
                {
                    log.LogError($"Failed to extract BSR number from: {bsrSection}.");
                }
            }
            else
            {
                log.LogError($"Bad HTTP request.");
            }
        }
    }
}
