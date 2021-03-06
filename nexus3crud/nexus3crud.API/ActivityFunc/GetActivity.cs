using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using nexus3crud.API.DTO;
using nexus3crud.BLL;
using nexus3crud.DAL.Model;
using nexus3crud.DAL.Repository;


namespace nexus3crud.API.ActivityFunc
{
    public class GetActivity
    {
        private readonly CosmosClient _cosmosClient;
        private readonly ActivityService activityservice;
        private IMapper mapper { get; set; }

        public GetActivity(CosmosClient client)
        {
            _cosmosClient = client;

            activityservice = new ActivityService(new Repositories.ActivityRepository(_cosmosClient, "Course"));

            if (mapper == null)
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<Activity, ActivityDTO>();
                    cfg.CreateMap<ActivityDTO, Activity>();
                });
                mapper = config.CreateMapper();
            }
        }

        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ActivityListDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [RequestHttpHeader("Authorization", isRequired: false)]
        [RequestHttpHeader("ContinuationToken", isRequired: false)]
        [FunctionName("GetAllActivity")]
        public async Task<IActionResult> GetAllActivityFunc(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "activity")] HttpRequest req,
            ILogger log)
        {
            var activities = await activityservice.GetAllActivity();

            var activityList = new List<ActivityDTO>();

            foreach (var item in activities.Items)
            {
                activityList.Add(mapper.Map<ActivityDTO>(item));
            }

            var result = new ActivityListDTO();

            result.ContinuationToken = activities.ContinuationToken;
            result.Items = activityList;

            return new OkObjectResult(result);
        }

        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ActivityDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [RequestHttpHeader("Authorization", isRequired: false)]
        [FunctionName("GetActivityById")]
        public async Task<IActionResult> GetActivityByIdFunc(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "activity/{id}")] ActivityDTO req,
           ILogger log, string id)
        {
            var activityInfo = await activityservice.GetActivityById(id);

            var result = new ActivityDTO();
            result = mapper.Map<ActivityDTO>(activityInfo);

            return new OkObjectResult(result);
        }
    }
}

