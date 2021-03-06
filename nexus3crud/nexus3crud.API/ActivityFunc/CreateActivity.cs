using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using nexus3crud.BLL;
using nexus3crud.DAL.Model;
using nexus3crud.DAL.Repository;

namespace nexus3crud.API.ActivityFunc
{
    public class CreateActivity
    {
        private readonly CosmosClient _cosmosClient;
        private readonly ActivityService activityservice;

        public CreateActivity(CosmosClient client)
        {
            _cosmosClient = client;

            activityservice = new ActivityService(new Repositories.ActivityRepository(_cosmosClient, "Course"));
        }

        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ActivityDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [RequestHttpHeader("Authorization", isRequired: true)]
        [FunctionName("CreateActivity")]
        public async Task<IActionResult> CreateActivityFunc(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "activity")]
            [RequestBodyType(typeof(ActivityDTO), "Create Activity request")] ActivityDTO req,
            ILogger log)
        {
            var act = new Activity()
            {
                Id = Guid.NewGuid().ToString(),
                ActivityName = req.ActivityName,
                Description = req.Description
            };

            var result = await activityservice.CreateNewActivity(act);

            var configToDto = new MapperConfiguration(cfg => {
                cfg.CreateMap<Activity, ActivityDTO>();
            });
            IMapper mapperToDto = new Mapper(configToDto);

            return new OkObjectResult(mapperToDto.Map<ActivityDTO>(result));
        }
    }
}

