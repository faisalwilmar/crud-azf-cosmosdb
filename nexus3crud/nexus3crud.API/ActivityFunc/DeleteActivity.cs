using System.IO;
using System.Net;
using System.Threading.Tasks;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using nexus3crud.BLL;
using nexus3crud.DAL.Repository;

namespace nexus3crud.API.ActivityFunc
{
    public class DeleteActivity
    {
        private readonly CosmosClient _cosmosClient;
        private readonly ActivityService activityservice;

        public DeleteActivity(CosmosClient client)
        {
            _cosmosClient = client;

            activityservice = new ActivityService(new Repositories.ActivityRepository(_cosmosClient, "Course"));
        }

        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [RequestHttpHeader("Authorization", isRequired: true)]
        [FunctionName("DeleteActivity")]
        public async Task<IActionResult> DeleteActivityFunction(
                [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "Activity/{id}")] HttpRequest req,
                ILogger log, string id)
        {
            var result = await activityservice.DeleteActivity(id);

            if (result.Contains("Activity Not Found"))
            {
                return new NotFoundObjectResult(result);
            }

            return new OkObjectResult(result);
        }
    }
}

