using Nexus.Base.CosmosDBRepository;
using nexuscrud.DAL.Model;
using System;

namespace nexuscrud.DAL.RepositoryAccess
{
    public class Repositories
    {
        private static readonly string C_CosmosDBEndpoint = Environment.GetEnvironmentVariable("CosmosDBEndPoint");
        private static readonly string C_CosmosDBKey = Environment.GetEnvironmentVariable("CosmosDBKey");
        private static readonly string C_EventGridEndPoint = Environment.GetEnvironmentVariable("EventGridEndPoint");
        private static readonly string C_EventGridKey = Environment.GetEnvironmentVariable("EventGridEndKey");

        public class ActivityRepository : DocumentDBRepository<Activity>
        {
            public ActivityRepository() :
                base(databaseId: "Course", endPoint: C_CosmosDBEndpoint, key: C_CosmosDBKey, createDatabaseIfNotExist: false,
                    eventGridEndPoint: C_EventGridEndPoint, eventGridKey: C_EventGridKey)
            { }
        }

        public class NotificationActivityRepository : DocumentDBRepository<NotificationActivity>
        {
            public NotificationActivityRepository() :
                base(databaseId: "Course", endPoint: C_CosmosDBEndpoint, key: C_CosmosDBKey, createDatabaseIfNotExist: false)
            { }
        }
    }
}
