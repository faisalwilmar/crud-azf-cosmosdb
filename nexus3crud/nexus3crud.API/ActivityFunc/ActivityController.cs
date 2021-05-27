using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using nexus3crud.BLL;
using nexus3crud.DAL.Repository;
using Microsoft.Azure.Cosmos;
using System.Net;
using nexus3crud.BLL.DTO;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using nexus3crud.DAL.Model;
using System.Collections.Generic;

namespace nexus3crud.API.ActivityFunc
{
    public class ActivityController
    {
        private readonly ActivityService activityservice;
        private static IUnitOfWork uow;

        public ActivityController(CosmosClient client)
        {
            uow ??= new UnitOfWork(client, "Course");

            activityservice ??= new ActivityService(uow);
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

            return new OkObjectResult(result);
        }

        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [RequestHttpHeader("Authorization", isRequired: true)]
        [FunctionName("DeleteActivity")]
        public async Task<IActionResult> DeleteActivityFunction(
                [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "Activity/{id}")] HttpRequest req,
                ILogger log, string id)
        {
            var result = await activityservice.DeleteActivity(id);

            if (result.Contains("Activity Not Found"))
                return new NotFoundObjectResult(result);

            return new OkObjectResult(result);
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
            var result = await activityservice.UpdateActivity(id, req);

            return new OkObjectResult(result);
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
            var result = await activityservice.GetAllActivity();

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
            var result = await activityservice.GetActivityById(id);

            return new OkObjectResult(result);
        }
    }
}
