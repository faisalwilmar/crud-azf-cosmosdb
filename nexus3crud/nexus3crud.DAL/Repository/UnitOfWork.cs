using Microsoft.Azure.Cosmos;
using Nexus.Base.CosmosDBRepository;
using nexus3crud.DAL.Model;
using System;

namespace nexus3crud.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private static readonly string C_EventGridEndPoint = Environment.GetEnvironmentVariable("EventGridEndPoint");
        private static readonly string C_EventGridKey = Environment.GetEnvironmentVariable("EventGridEndKey");
        
        private string DB;

        private readonly CosmosClient _client;

        private readonly Lazy<IDocumentDBRepository<Activity>> activityRepository;
        private readonly Lazy<IDocumentDBRepository<NotificationActivity>> notificationAactivityRepository;

        public UnitOfWork(CosmosClient client, string database)
        {
            this._client = client;
            this.DB = database;

            if(this.activityRepository == null)
            {
                this.activityRepository = new Lazy<IDocumentDBRepository<Activity>>(new DocumentDBRepository<Activity>
                        (DB, _client, eventGridEndPoint: C_EventGridEndPoint, eventGridKey: C_EventGridKey));
            }

            if(this.notificationAactivityRepository == null)
            {
                this.notificationAactivityRepository = new Lazy<IDocumentDBRepository<NotificationActivity>>(new DocumentDBRepository<NotificationActivity>
                    (DB, _client, eventGridEndPoint: C_EventGridEndPoint, eventGridKey: C_EventGridKey));
            }   
        }

        public IDocumentDBRepository<Activity> ActivityRepository => this.activityRepository.Value;

        public IDocumentDBRepository<NotificationActivity> NotificationActivityRepository => this.notificationAactivityRepository.Value;

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
