using AutoMapper;
using Nexus.Base.CosmosDBRepository;
using nexus3crud.BLL.DTO;
using nexus3crud.DAL.Model;
using nexus3crud.DAL.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nexus3crud.BLL
{
    public class ActivityService
    {
        //private readonly IDocumentDBRepository<Activity> _repository;

        private readonly IUnitOfWork uow;
        private readonly IMapper _mapper;

        public ActivityService(IUnitOfWork uow)
        {
            this.uow ??= uow;
            if(_mapper == null)
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<Activity, ActivityDTO>();
                    cfg.CreateMap<ActivityDTO, Activity>();
                });

                _mapper = config.CreateMapper();
            }
        }

        public async Task<ActivityDTO> GetActivityById(string id)
        {
            return _mapper.Map<ActivityDTO>(await uow.ActivityRepository.GetByIdAsync(id));
        }

        public async Task<ActivityListDTO> GetAllActivity()
        {
            var activities = await uow.ActivityRepository.GetAsync(p => true);
            
            var activityList = new List<ActivityDTO>();

            foreach (var item in activities.Items)
            {
                activityList.Add(_mapper.Map<ActivityDTO>(item));
            }

            var result = new ActivityListDTO();
            result.ContinuationToken = activities.ContinuationToken;
            result.Items = activityList;

            return result;
        }

        public async Task<ActivityDTO> CreateNewActivity(Activity act)
        {
            return _mapper.Map<ActivityDTO>(await uow.ActivityRepository.CreateAsync(act));
        }

        public async Task<ActivityDTO> UpdateActivity(string Id, ActivityDTO act)
        {
            var activity = _mapper.Map<Activity>(act);
            var ret = await uow.ActivityRepository.UpdateAsync(Id, activity);
            return _mapper.Map<ActivityDTO>(ret);
        }

        public async Task<string> DeleteActivity(string Id)
        {
            try
            {
                await uow.ActivityRepository.DeleteAsync(Id);
                return "Activity Deleted";
            }
            catch
            {
                return "Activity Not Found";
            }
        }
    }
}
