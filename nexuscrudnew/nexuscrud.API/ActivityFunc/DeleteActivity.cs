using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using nexuscrud.DAL.RepositoryAccess;

namespace nexuscrud.ActivityFunc
{
    public static class DeleteActivity
    {
        [FunctionName("DeleteActivity")]
        public static async Task<IActionResult> DeleteCategory(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "Activity/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            using (var reps = new Repositories.ActivityRepository())
            {
                await reps.DeleteAsync(id);
            }

            var result = "Delete success";
            return new OkObjectResult(result);
        }
    }
}

