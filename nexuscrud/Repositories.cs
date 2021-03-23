using Nexus.Base.CosmosDBRepository;
using nexuscrud.Model;
using System;

namespace nexuscrud.DataAccess
{
    public class Repositories
    {
        private static readonly string C_CosmosDBEndpoint = Environment.GetEnvironmentVariable("CosmosDBEndPoint");
        private static readonly string C_CosmosDBKey = Environment.GetEnvironmentVariable("CosmosDBKey");

        public class ActivityRepository : DocumentDBRepository<Activity>
        {
            public ActivityRepository() :
                base(databaseId: "Course", endPoint: C_CosmosDBEndpoint, key: C_CosmosDBKey, createDatabaseIfNotExist: false)
            { }
        }
    }
}
