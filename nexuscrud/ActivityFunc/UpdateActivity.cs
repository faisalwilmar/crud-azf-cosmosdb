using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using nexuscrud.DataAccess;
using nexuscrud.DTO;
using nexuscrud.Model;

namespace nexuscrud.ActivityFunc
{
    public static class UpdateActivity
    {
        [FunctionName("UpdateActivity")]
        public static async Task<IActionResult> UpdateActivityFunc(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Activity/{id}")] ActivityDTO req,
            ILogger log, string id)
        {

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<ActivityDTO, Activity>();
            });

            IMapper mapper = new Mapper(config);
            var item = mapper.Map<Activity>(req);
            Document result;

            using (var reps = new Repositories.ActivityRepository())
            {
                result = await reps.UpdateAsync(id, item);
            }

            return new OkObjectResult(result);
        }
    }
}

