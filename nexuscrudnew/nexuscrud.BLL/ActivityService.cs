using Microsoft.Azure.Documents;
using Newtonsoft.Json.Linq;
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

        public async Task<PageResult<Activity>> GetAllActivity()
        {
            return await _repository.GetAsync(predicate: null);
        }

        public async Task<Activity> CreateNewActivity(Activity act)
        {
            var returnVal = await _repository.CreateAsync(act);
            JObject json = JObject.Parse(returnVal.ToString());
            Activity activ = json.ToObject<Activity>();
            return activ;
        }

        public async Task<Activity> UpdateActivity(string Id, Activity act)
        {
            var returnVal = await _repository.UpdateAsync(Id, act);
            JObject json = JObject.Parse(returnVal.ToString());
            Activity activ = json.ToObject<Activity>();
            return activ;
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
