using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using nexuscrud.DAL.Model;
using nexuscrud.DAL.RepositoryAccess;

namespace nexuscrud.NotificationFunc
{
    public static class Notification
    {
        [FunctionName("Notification")]
        public static async Task Run([EventHubTrigger("activity.notification", Connection = "EventHubConnectionString")] EventData[] events, ILogger log)
        {
            var exceptions = new List<Exception>();

            foreach (EventData eventData in events)
            {
                try
                {
                    string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                    // Replace these two lines with your processing logic.
                    log.LogInformation($"C# Event Hub trigger function processed a message: {messageBody}");
                    var actNot = new NotificationActivity()
                    {
                        Id = Guid.NewGuid().ToString(),
                        MessageBody = messageBody,
                        UtcTime = DateTime.UtcNow
                    };

                    using (var reps = new Repositories.NotificationActivityRepository())
                    {
                        await reps.CreateAsync(actNot);
                    }
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }
    }
}
