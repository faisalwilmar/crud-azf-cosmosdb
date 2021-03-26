using System;
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
using nexuscrud.DAL.Model;
using nexuscrud.DAL.RepositoryAccess;
using nexuscrud.DTO;

namespace nexuscrud.ActivityFunc
{
    public static class CreateActivity
    {
        [FunctionName("CreateActivity")]
        public static async Task<IActionResult> CreateActivityFunc(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "activity")] ActivityDTO req,
            ILogger log)
        {
            var act = new Activity()
            {
                Id = Guid.NewGuid().ToString(),
                ActivityName = req.ActivityName,
                Description = req.Description
            };

            Document result;

            using (var reps = new Repositories.ActivityRepository())
            {
                result = await reps.CreateAsync(act);
            }

            return new OkObjectResult(result);
        }
    }
}

