using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
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
    public class UpdateActivity
    {
        private readonly CosmosClient _cosmosClient;

        public UpdateActivity(CosmosClient client)
        {
            _cosmosClient = client;
        }

        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ActivityDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [RequestHttpHeader("Authorization", isRequired: true)]
        [FunctionName("UpdateActivity")]
        public async Task<IActionResult> UpdateActivityFunc(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Activity/{id}")]
            [RequestBodyType(typeof(ActivityDTO), "Update Activity request")]ActivityDTO req,
            ILogger log, string id)
        {

            var configFromDto = new MapperConfiguration(cfg => {
                cfg.CreateMap<ActivityDTO, Activity>();
            });
            IMapper mapperFromDto = new Mapper(configFromDto);

            var item = mapperFromDto.Map<Activity>(req);

            ActivityService activityservice = new ActivityService(new Repositories.ActivityRepository(_cosmosClient));

            var result = await activityservice.UpdateActivity(id, item);

            var configToDto = new MapperConfiguration(cfg => {
                cfg.CreateMap<Activity, ActivityDTO>();
            });
            IMapper mapperToDto = new Mapper(configToDto);

            return new OkObjectResult(mapperToDto.Map<ActivityDTO>(result));
        }
    }
}

