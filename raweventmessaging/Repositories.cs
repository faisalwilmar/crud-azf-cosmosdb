using Nexus.Base.CosmosDBRepository;
using raweventmessaging.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace raweventmessaging.DataAccess
{
    public class Repositories
    {
        private static readonly string C_CosmosDBEndpoint = Environment.GetEnvironmentVariable("CosmosDBEndPoint");
        private static readonly string C_CosmosDBKey = Environment.GetEnvironmentVariable("CosmosDBKey");
        private static readonly string C_EventGridEndPoint = Environment.GetEnvironmentVariable("EventGridEndPoint");
        private static readonly string C_EventGridKey = Environment.GetEnvironmentVariable("EventGridEndKey");

        public class NotificationGeneralRepository : DocumentDBRepository<NotificationGeneral>
        {
            public NotificationGeneralRepository() :
                base(databaseId: "Course", endPoint: C_CosmosDBEndpoint, key: C_CosmosDBKey, createDatabaseIfNotExist: false)
            { }
        }
    }
}
