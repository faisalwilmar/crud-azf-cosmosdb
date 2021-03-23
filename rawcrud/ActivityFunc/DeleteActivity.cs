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

namespace rawcrud.ActivityFunc
{
    public static class DeleteActivity
    {
        [FunctionName("DeleteActivity")]
        public static async Task<IActionResult> DeleteActivityFunc(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "activity/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: "Course",
                collectionName: "Activity",
                ConnectionStringSetting = "cosmos-bl-tutorial-serverless",
                Id = "{id}",
                PartitionKey = "Activity")] Document documentToDelete,
            [CosmosDB(
                databaseName: "Course",
                collectionName: "Activity",
                ConnectionStringSetting = "cosmos-bl-tutorial-serverless")] DocumentClient document,
            ILogger log)
        {
            if (documentToDelete == null)
            {
                return new BadRequestResult();
            }

            await document.DeleteDocumentAsync(documentToDelete.SelfLink, new RequestOptions() { PartitionKey = new PartitionKey("Activity") });

            return new OkObjectResult("Activity Deleted");
        }
    }
}

