using System.IO;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using nexuscrud.BLL;
using nexuscrud.DAL.Model;
using nexuscrud.DAL.RepositoryAccess;
using nexuscrud.DTO;

namespace nexuscrud.ActivityFunc
{
    public static class UpdateActivity
    {
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ActivityDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [RequestHttpHeader("Authorization", isRequired: true)]
        [FunctionName("UpdateActivity")]
        public static async Task<IActionResult> UpdateActivityFunc(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "Activity/{id}")]
            [RequestBodyType(typeof(ActivityDTO), "Update Activity request")]ActivityDTO req,
            [SwaggerIgnore][CosmosDB(ConnectionStringSetting = "cosmos-bl-tutorial-serverless")] DocumentClient client,
            ILogger log, string id)
        {

            var configFromDto = new MapperConfiguration(cfg => {
                cfg.CreateMap<ActivityDTO, Activity>();
            });
            IMapper mapperFromDto = new Mapper(configFromDto);
            
            var item = mapperFromDto.Map<Activity>(req);

            ActivityService activityservice = new ActivityService(new Repositories.ActivityRepository(client));

            var result = await activityservice.UpdateActivity(id, item);

            var configToDto = new MapperConfiguration(cfg => {
                cfg.CreateMap<Activity, ActivityDTO>();
            });
            IMapper mapperToDto = new Mapper(configToDto);

            return new OkObjectResult(mapperToDto.Map<ActivityDTO>(result));
        }
    }
}

