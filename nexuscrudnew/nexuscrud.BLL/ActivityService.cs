using Microsoft.Azure.Documents;
using Nexus.Base.CosmosDBRepository;
using nexuscrud.DAL.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nexuscrud.BLL
{
    public class ActivityService
    {
        private readonly IDocumentDBRepository<Activity> _repository;

        public ActivityService(IDocumentDBRepository<Activity> repository)
        {
            if (this._repository == null)
            {
                this._repository = repository;
            }
        }

        public async Task<Activity> GetActivityById(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<PageResult<object>> GetAllActivity()
        {
            return await _repository.GetAsync(sqlQuery: "select * from c");
        }

        public async Task<Document> CreateNewActivity(Activity act)
        {
            return await _repository.CreateAsync(act);
        }

        public async Task<Document> UpdateActivity(string Id, Activity act)
        {
            return await _repository.UpdateAsync(Id, act);
        }

        public async Task<string> DeleteActivity(string Id)
        {
            try
            {
                await _repository.DeleteAsync(Id);
                return "Activity Deleted";
            }
            catch
            {
                return "Activity Not Found";
            }
        }
    }
}
