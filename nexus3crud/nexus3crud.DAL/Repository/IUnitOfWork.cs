using System;
using Nexus.Base.CosmosDBRepository;
using nexus3crud.DAL.Model;

namespace nexus3crud.DAL.Repository
{
    public interface IUnitOfWork: IDisposable
    {
        IDocumentDBRepository<Activity> ActivityRepository { get; }
        IDocumentDBRepository<NotificationActivity> NotificationActivityRepository { get; }
    }
}
