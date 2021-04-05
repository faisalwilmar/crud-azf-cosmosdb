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

        public GetActivity(CosmosClient client)
        {
            _cosmosClient = client;
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
            // TODO: coba baca2 apakah best practice pemakaian mapper seperti ini?
            //       krn setiap kali pemanggilan function, mapper akan terbentuk.
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Activity, ActivityDTO>();
            });

            IMapper mapper = new Mapper(config);

            // TODO: coba baca2 apakah best practice pemakaian object seperti ini?
            //       krn setiap kali pemanggilan function, object akan terbentuk.
            ActivityService activityservice = new ActivityService(new Repositories.ActivityRepository(_cosmosClient));

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
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Activity, ActivityDTO>();
            });

            IMapper mapper = new Mapper(config);

            ActivityService activityservice = new ActivityService(new Repositories.ActivityRepository(_cosmosClient));

            var activityInfo = await activityservice.GetActivityById(id);

            var result = new ActivityDTO();
            result = mapper.Map<ActivityDTO>(activityInfo);

            return new OkObjectResult(result);
        }
    }
}

