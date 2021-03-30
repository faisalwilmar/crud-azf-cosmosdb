using Nexus.Base.CosmosDBRepository;
using nexus3crud.DAL.Model;
using System;
using System.Threading.Tasks;

namespace nexus3crud.BLL
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
            return await _repository.GetAsync();
        }

        public async Task<Activity> CreateNewActivity(Activity act)
        {
            return await _repository.CreateAsync(act);
            
        }

        public async Task<Activity> UpdateActivity(string Id, Activity act)
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
