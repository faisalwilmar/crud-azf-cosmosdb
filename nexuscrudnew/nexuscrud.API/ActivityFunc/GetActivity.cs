using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using nexuscrud.BLL;
using nexuscrud.DAL.Model;
using nexuscrud.DAL.RepositoryAccess;
using nexuscrud.DTO;

namespace nexuscrud
{
    public class GetActivity
    {
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetAllActivityResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [RequestHttpHeader("Authorization", isRequired: false)]
        [RequestHttpHeader("ContinuationToken", isRequired: false)]
        [FunctionName("GetAllActivity")]
        public static async Task<IActionResult> GetAllActivityFunc(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "activity")] HttpRequest req,
            [SwaggerIgnore][CosmosDB(ConnectionStringSetting = "cosmos-bl-tutorial-serverless")] DocumentClient client,
            ILogger log)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Activity, ActivityDTO>();
            });

            IMapper mapper = new Mapper(config);

            ActivityService activityservice = new ActivityService(new Repositories.ActivityRepository(client));

            var activities = await activityservice.GetAllActivity();

            var activityList = new List<ActivityDTO>();

            foreach (var item in activities.Items)
            {
                JObject json = JObject.Parse(item.ToString());
                Activity activ = json.ToObject<Activity>();
                activityList.Add(mapper.Map<ActivityDTO>(activ));
            }

            var result = new GetAllActivityResponse();

            result.ContinuationToken = activities.ContinuationToken;
            result.Items = activityList;

            return new OkObjectResult(result);
        }

        class GetAllActivityResponse
        {
            public string ContinuationToken { get; set; }
            public List<ActivityDTO> Items { get; set; }
        }

        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ActivityDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [RequestHttpHeader("Authorization", isRequired: false)]
        [FunctionName("GetActivityById")]
        public async Task<IActionResult> GetActivityByIdFunc(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "activity/{id}")] ActivityDTO req,
           [SwaggerIgnore][CosmosDB(ConnectionStringSetting = "cosmos-bl-tutorial-serverless")] DocumentClient client,
           ILogger log, string id)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Activity, ActivityDTO>();
            });

            IMapper mapper = new Mapper(config);

            ActivityService activityservice = new ActivityService(new Repositories.ActivityRepository(client));

            var activityInfo = await activityservice.GetActivityById(id);

            var result = new ActivityDTO();
            result = mapper.Map<ActivityDTO>(activityInfo);

            return new OkObjectResult(result);
        }
    }
}

