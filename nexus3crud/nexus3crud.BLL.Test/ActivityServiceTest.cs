using Microsoft.Azure.Documents;
using Moq;
using Newtonsoft.Json;
using Nexus.Base.CosmosDBRepository;
using nexus3crud.BLL.DTO;
using nexus3crud.DAL.Model;
using nexus3crud.DAL.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace nexus3crud.BLL.Test
{
    public static class ActivityServiceTest
    {
        public static IEnumerable<Activity> activities = new List<Activity>
            {
                {new Activity() { Id = "354806d7-a5b7-451c-8a16-3212c4c03a95", ActivityName = "Activity to String", Description = "Description of Activity for delete"} },
                {new Activity() { Id = "cf2f68d1-361f-4b3e-944b-d0d5806af395", ActivityName = "Activity to String to del", Description = "Description of Activity for delete"} },
                {new Activity() { Id = "22a7f1e3-6783-47ea-b499-43eeb8569a32", ActivityName = "Activity to String to del XXX", Description = "Description of Activity for delete"} }
            };

        private static Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();

        static ActivityServiceTest()
        {
            Initiator(uow);
        }

        public static void Initiator(Mock<IUnitOfWork> uow)
        {
            uow.Setup(c => c.ActivityRepository.GetByIdAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()
                )).ReturnsAsync((string id, Dictionary<string, string> dict) => activities.Where(o => o.Id == id).FirstOrDefault());
            
            uow.Setup(c => c.ActivityRepository.GetAsync(
                    It.IsAny<Expression<Func<Activity, bool>>>(),
                    It.IsAny<Func<IQueryable<Activity>, IOrderedQueryable<Activity>>>(),
                    It.IsAny<Expression<Func<Activity, Activity>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<Dictionary<string, string>>()
                )).ReturnsAsync((
                Expression<Func<Activity, bool>> predicate,
                Func<IQueryable<Activity>, IOrderedQueryable<Activity>> orderBy,
                Expression<Func<Activity, Activity>> selector,
                bool usePaging, string continuationToken, int pageSize, Dictionary<string, string> pk
            ) => 
                new PageResult<Activity>(predicate != null ? activities.Where(predicate?.Compile()) : activities, ""));

            uow.Setup(c => c.ActivityRepository.CreateAsync(
                It.IsAny<Activity>(),
                It.IsAny<EventGridOptions>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            )).ReturnsAsync((Activity p, EventGridOptions evg, string str1, string str2) => p);

            uow.Setup(c => c.ActivityRepository.UpdateAsync(
                It.IsAny<string>(),
                It.IsAny<Activity>(),
                It.IsAny<EventGridOptions>(),
                It.IsAny<string>()
            )).ReturnsAsync((string id, Activity p, EventGridOptions evg, string str2) => p);

            //uow.Setup(c => c.ActivityRepository.DeleteAsync(
            //    It.IsAny<string>(),
            //    It.IsAny<Dictionary<string, string>>(),
            //    It.IsAny<EventGridOptions>()
            //)).Callback((string id, Dictionary<string, string> p, EventGridOptions evg) =>
            //{
            //    activities = activities.Where(p => p.Id != id).ToList();
            //}); kalo ini di delete beneran, cara cek nya diitung isi itemnya

            uow.Setup(c => c.ActivityRepository.DeleteAsync(
                   It.IsAny<string>(),
                   It.IsAny<Dictionary<string, string>>(),
                   It.IsAny<EventGridOptions>()
               ));

        }

        public class GetActivityById
        {
            [Theory]
            [InlineData("354806d7-a5b7-451c-8a16-3212c4c03a95")]
            [InlineData("22a7f1e3-6783-47ea-b499-43eeb8569a32")]
            public async Task GetActivityById_Success(string id)
            {
                // arrange
                var svc = new ActivityService(uow.Object);

                var output = activities.Where(o => o.Id == id).FirstOrDefault();

                var desiredOutput = new ActivityDTO()
                {
                    Id = output.Id,
                    ActivityName = output.ActivityName,
                    Description = output.Description
                };

                // act
                var act = await svc.GetActivityById(id);

                // assert
                Assert.Equal(desiredOutput.ActivityName, act.ActivityName);
                Assert.Equal(desiredOutput.Description, act.Description);
                Assert.IsType<ActivityDTO>(act);
            }

            [Fact]
            public async Task GetAllActivity_Success()
            {
                // arrange
                var svc = new ActivityService(uow.Object);

                // act
                var act = await svc.GetAllActivity();

                // assert
                Assert.IsType<ActivityListDTO>(act);
            }
        }

        public class CreateActivity
        {
            [Fact]
            public async Task CreateActivity_Success()
            {
                // arrange
                var repo = new Mock<IDocumentDBRepository<Activity>>();

                var GeneratedId = "1515faa2-132e-4a4d-8efb-57ac3891c00f";

                var activityNew = new Activity()
                {
                    Id = GeneratedId,
                    ActivityName = "The New Activity",
                    Description = "Activity Description"
                };

                var svc = new ActivityService(uow.Object);

                // act
                var act = await svc.CreateNewActivity(activityNew);

                // assert
                Assert.Equal(activityNew.Id, act.Id);
                Assert.Equal(activityNew.ActivityName, act.ActivityName);
                Assert.Equal(activityNew.Description, act.Description);
                Assert.IsType<ActivityDTO>(act);
            }
        }

        public class UpdateActivity
        {
            [Fact]
            public async Task UpdateActivity_Success()
            {
                // arrange
                var repo = new Mock<IDocumentDBRepository<Activity>>();

                var GeneratedId = "354806d7-a5b7-451c-8a16-3212c4c03a95";

                var activityNew = new ActivityDTO()
                {
                    Id = GeneratedId,
                    ActivityName = "The New Activity",
                    Description = "Activity Description"
                };

                var svc = new ActivityService(uow.Object);

                // act
                var act = await svc.UpdateActivity(GeneratedId, activityNew);

                // assert
                Assert.Equal(GeneratedId, act.Id);
                Assert.Equal(activityNew.ActivityName, act.ActivityName);
                Assert.Equal(activityNew.Description, act.Description);
                Assert.IsType<ActivityDTO>(act);
            }
        }

        public class DeleteActivity
        {
            [Fact]
            public async Task DeleteActivity_Success()
            {
                // arrange
                var repo = new Mock<IDocumentDBRepository<Activity>>();

                var GeneratedId = Guid.NewGuid().ToString();

                var svc = new ActivityService(uow.Object);

                // act
                var act = await svc.DeleteActivity(GeneratedId);

                // assert
                Assert.Equal("Activity Deleted", act);
            }
        }
    }
}
