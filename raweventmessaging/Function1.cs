using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using raweventmessaging.DTO;

namespace raweventmessaging
{
    public static class Function1
    {
        [FunctionName("PostData")]
        public static async Task<IActionResult> PostDataFunc(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "data")] MessageRequestDTO req,
            [EventGrid(TopicEndpointUri = "EventGridEndPoint", TopicKeySetting = "EventGridEndKey")] IAsyncCollector<EventGridEvent> outputEvents,
            ILogger log)
        {
            try
            {
                //var myEvent = new EventGridEvent("message-id-", "subject-name", "event-data", "event-type", DateTime.UtcNow, "1.0");
                var myEvent = new EventGridEvent(req.Id, "raw messaging", req.MessageRequest, "raweventmessaging", DateTime.UtcNow, "1.0");
                await outputEvents.AddAsync(myEvent);
                return new OkObjectResult(myEvent);
            }
            catch
            {
                return new BadRequestObjectResult("I dont know what happen");
            }
            
        }
    }
}

