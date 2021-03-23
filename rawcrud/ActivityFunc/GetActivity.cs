using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using rawcrud.Model;

namespace rawcrud
{
    public static class GetActivity
    {
        [FunctionName("GetAllActivity")]
        public static async Task<IActionResult> GetAllActivityFunc(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "activity")] HttpRequest req,
            [CosmosDB(ConnectionStringSetting = "cosmos-bl-tutorial-serverless")] DocumentClient documentClient,
            ILogger log)
        {
            var query = new SqlQuerySpec("SELECT * FROM c");
            var pk = new PartitionKey("Activity");
            var options = new FeedOptions() { PartitionKey = pk };
            var data = documentClient.CreateDocumentQuery(UriFactory.CreateDocumentCollectionUri("Course", "Activity"), query, options);
            return new OkObjectResult(data);
        }

        [FunctionName("GetActivityById")]
        public static IActionResult GetActivityByIdFunc(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "activity/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: "Course",
                collectionName: "Activity",
                ConnectionStringSetting = "cosmos-bl-tutorial-serverless",
                Id = "{id}",
                PartitionKey = "Activity")] Activity activity,
            ILogger log)
        {
            return new OkObjectResult(activity);
        }
    }
}

