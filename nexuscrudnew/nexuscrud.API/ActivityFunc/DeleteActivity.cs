using System.IO;
using System.Net;
using System.Threading.Tasks;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using nexuscrud.BLL;
using nexuscrud.DAL.RepositoryAccess;

namespace nexuscrud.ActivityFunc
{
    public static class DeleteActivity
    {
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [RequestHttpHeader("Authorization", isRequired: true)]
        [FunctionName("DeleteActivity")]
        public static async Task<IActionResult> DeleteCategory(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "Activity/{id}")] HttpRequest req,
            [CosmosDB(ConnectionStringSetting = "cosmos-bl-tutorial-serverless")] DocumentClient client,
            ILogger log, string id)
        {
            ActivityService activityservice = new ActivityService(new Repositories.ActivityRepository(client));

            var result = await activityservice.DeleteActivity(id);

            if(result.Contains("Activity Not Found"))
            {
                return new NotFoundObjectResult(result);
            }

            return new OkObjectResult(result);
        }
    }
}

