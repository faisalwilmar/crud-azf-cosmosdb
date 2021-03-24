// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;

namespace raweventmessaging
{
    public static class Function2
    {
        [FunctionName("TriggerToEventHub")]
        [return: EventHub("activity.notification", Connection = "EventHubConnectionString")]
        public static string Run([EventGridTrigger]EventGridEvent eventGridEvent,
            ILogger log)
        {
            log.LogInformation(eventGridEvent.Data.ToString());
            return eventGridEvent.Data.ToString();
        }
    }
}
