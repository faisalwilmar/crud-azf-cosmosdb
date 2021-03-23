using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using nexuscrud.DataAccess;
using nexuscrud.DTO;
using nexuscrud.Model;

namespace nexuscrud
{
    public class GetActivity
    {
        [FunctionName("GetAllActivity")]
        public static async Task<IActionResult> GetAllActivityFunc(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "activity")] HttpRequest req,
            ILogger log)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Activity, ActivityDTO>();
            });

            IMapper mapper = new Mapper(config);

            using(var reps = new Repositories.ActivityRepository())
            {
                var activities = await reps.GetAsync();
                var result = new List<ActivityDTO>();

                foreach (var item in activities.Items)
                {
                    result.Add(mapper.Map<ActivityDTO>(item));
                }

                return new OkObjectResult(
                    new
                    {
                        ContinuationToken = activities.ContinuationToken,
                        Items = result
                    });
            }
        }

        [FunctionName("GetActivityById")]
        public async Task<IActionResult> GetActivityByIdFunc(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "activity/{id}")] ActivityDTO req,
           ILogger log, string id)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Activity, ActivityDTO>();
            });

            IMapper mapper = new Mapper(config);

            using var reps = new Repositories.ActivityRepository();

            var activityInfo = await reps.GetByIdAsync(id);

            var result = mapper.Map<ActivityDTO>(activityInfo);

            return new OkObjectResult(result);
        }
    }
}

