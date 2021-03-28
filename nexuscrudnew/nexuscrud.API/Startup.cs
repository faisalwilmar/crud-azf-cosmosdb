using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Azure.WebJobs;
using AzureFunctions.Extensions.Swashbuckle;
using System.Reflection;

[assembly: WebJobsStartup(typeof(nexuscrud.API.Startup))]
namespace nexuscrud.API
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
        }
    }
}
