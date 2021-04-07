using Microsoft.Azure.Cosmos;
using Nexus.Base.CosmosDBRepository;
using nexus3crud.DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace nexus3crud.DAL.Repository
{
    public class Repositories
    {
        private static readonly string C_CosmosDBEndpoint = Environment.GetEnvironmentVariable("CosmosDBEndPoint");
        private static readonly string C_CosmosDBKey = Environment.GetEnvironmentVariable("CosmosDBKey");
        private static readonly string C_EventGridEndPoint = Environment.GetEnvironmentVariable("EventGridEndPoint");
        private static readonly string C_EventGridKey = Environment.GetEnvironmentVariable("EventGridEndKey");

        public class ActivityRepository : DocumentDBRepository<Activity>
        {
            //Cara 1 define repository
            public ActivityRepository(CosmosClient client, string database) :
                base(databaseId: database, client, createDatabaseIfNotExist: false,
                    eventGridEndPoint: C_EventGridEndPoint, eventGridKey: C_EventGridKey)
            { }
        }

        public class NotificationActivityRepository : DocumentDBRepository<NotificationActivity>
        {
            //Cara 2 define repository
            public NotificationActivityRepository(string database) :
                base(databaseId: database, endPoint: C_CosmosDBEndpoint, key: C_CosmosDBKey, createDatabaseIfNotExist: false)
            { }
        }
    }
}
