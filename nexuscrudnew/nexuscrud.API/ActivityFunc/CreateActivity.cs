using System;
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
    public static class CreateActivity
    {
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Document))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [RequestHttpHeader("Authorization", isRequired: true)]
        [FunctionName("CreateActivity")]
        public static async Task<IActionResult> CreateActivityFunc(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "activity")]
            [RequestBodyType(typeof(ActivityDTO), "Create Activity request")] ActivityDTO req,
            [SwaggerIgnore][CosmosDB(ConnectionStringSetting = "cosmos-bl-tutorial-serverless")] DocumentClient client,
            ILogger log)
        {
            var act = new Activity()
            {
                Id = Guid.NewGuid().ToString(),
                ActivityName = req.ActivityName,
                Description = req.Description
            };

            //Document result;

            ActivityService activityservice = new ActivityService(new Repositories.ActivityRepository(client));

            var result = await activityservice.CreateNewActivity(act);

            var configToDto = new MapperConfiguration(cfg => {
                cfg.CreateMap<Activity, ActivityDTO>();
            });
            IMapper mapperToDto = new Mapper(configToDto);

            return new OkObjectResult(mapperToDto.Map<ActivityDTO>(result));
        }
    }
}

