using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using rawcrud.Model;

namespace rawcrud.ActivityFunc
{
    public static class UpdateActivity
    {
        [FunctionName("UpdateActivity")]
        public static IActionResult UpdateActivityFunc(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "activity")] HttpRequestMessage req,
            [CosmosDB(
            databaseName: "Course",
                collectionName: "Activity",
                ConnectionStringSetting = "cosmos-bl-tutorial-serverless")] out dynamic activityData,
            ILogger log)
        {
            ObjectResult result;
            activityData = req.Content.ReadAsAsync<Activity>().Result;

            if(activityData != null)
            {
                result = new OkObjectResult("Activity Updated");
            } else
            {
                result = new BadRequestObjectResult("Invalid Id");
            }

            return new ObjectResult(result);
        }
    }
}

