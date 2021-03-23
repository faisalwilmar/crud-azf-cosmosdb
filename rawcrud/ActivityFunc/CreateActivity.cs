using System;
using System.IO;
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
    public static class CreateActivity
    {
        [FunctionName("CreateActivity")]
        public static async Task<IActionResult> CreateActivityFunc(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "activity")] HttpRequest req,
            [CosmosDB(
                databaseName: "Course",
                collectionName: "Activity",
                ConnectionStringSetting = "cosmos-bl-tutorial-serverless")] IAsyncCollector<object> activities,
            ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                var input = JsonConvert.DeserializeObject<Activity>(requestBody);

                var activity = new Activity
                {
                    Id = Guid.NewGuid().ToString(),
                    ActivityName = input.ActivityName,
                    Description = input.Description,
                    PartitionKey = "Activity"
                };

                await activities.AddAsync(activity);

                return new OkObjectResult(activity);
            }
            catch (Exception ex)
            {
                log.LogError($"Couldn't insert item. Exception thrown: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

