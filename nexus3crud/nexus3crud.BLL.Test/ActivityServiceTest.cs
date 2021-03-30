using Microsoft.Azure.Documents;
using Moq;
using Newtonsoft.Json;
using Nexus.Base.CosmosDBRepository;
using nexus3crud.DAL.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace nexus3crud.BLL.Test
{
    public class ActivityServiceTest
    {
        public class GetActivityById
        {
            readonly static IEnumerable<Activity> activities = new List<Activity>
            {
                {new Activity() { Id = "354806d7-a5b7-451c-8a16-3212c4c03a95", ActivityName = "Activity to String", Description = "Description of Activity for delete"} },
                {new Activity() { Id = "cf2f68d1-361f-4b3e-944b-d0d5806af395", ActivityName = "Activity to String to del", Description = "Description of Activity for delete"} },
                {new Activity() { Id = "22a7f1e3-6783-47ea-b499-43eeb8569a32", ActivityName = "Activity to String to del XXX", Description = "Description of Activity for delete"} }
            };

            [Theory]
            [InlineData("354806d7-a5b7-451c-8a16-3212c4c03a95")]
            [InlineData("354806d7-a5b7-451c-8a16-3212c4c03xxx")]
            public async Task GetActivityById_Success(string id)
            {
                // arrange
                var repo = new Mock<IDocumentDBRepository<Activity>>();

                var output = activities.Where(o => o.Id == id).FirstOrDefault();

                repo.Setup(c => c.GetByIdAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()
                )).Returns(
                    Task.FromResult<Activity>(output)
                );

                var svc = new ActivityService(repo.Object);

                // act
                var act = await svc.GetActivityById(id);

                // assert
                Assert.Equal(output, act);
            }

            [Fact]
            public async Task GetAllActivity_Success()
            {
                // arrange
                var repo = new Mock<IDocumentDBRepository<Activity>>();

                var output = new PageResult<Activity>(activities, "");

                repo.Setup(c => c.GetAsync(
                    It.IsAny<Expression<Func<Activity, bool>>>(),
                    It.IsAny<Func<IQueryable<Activity>, IOrderedQueryable<Activity>>>(),
                    It.IsAny<Expression<Func<Activity, Activity>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<Dictionary<string, string>>()
                )).Returns(Task.FromResult(output));

                var svc = new ActivityService(repo.Object);

                // act
                var act = await svc.GetAllActivity();

                // assert
                Assert.Equal(output, act);
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

                repo.Setup(c => c.CreateAsync(
                    It.IsAny<Activity>(),
                    It.IsAny<EventGridOptions>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()
                )).Returns(
                    Task.FromResult<Activity>(activityNew)
                );

                var svc = new ActivityService(repo.Object);

                // act
                var act = await svc.CreateNewActivity(activityNew);

                // assert
                Assert.Equal(activityNew.Id, act.Id);
                Assert.Equal(activityNew.ActivityName, act.ActivityName);
                Assert.Equal(activityNew.Description, act.Description);
            }
        }

        public class UpdateActivity
        {
            readonly static IEnumerable<Activity> activities = new List<Activity>
            {
                {new Activity() { Id = "354806d7-a5b7-451c-8a16-3212c4c03a95", ActivityName = "Activity to String", Description = "Description of Activity for delete"} },
                {new Activity() { Id = "cf2f68d1-361f-4b3e-944b-d0d5806af395", ActivityName = "Activity to String to del", Description = "Description of Activity for delete"} },
                {new Activity() { Id = "22a7f1e3-6783-47ea-b499-43eeb8569a32", ActivityName = "Activity to String to del XXX", Description = "Description of Activity for delete"} }
            };

            [Fact]
            public async Task UpdateActivity_Success()
            {
                // arrange
                var repo = new Mock<IDocumentDBRepository<Activity>>();

                var GeneratedId = "354806d7-a5b7-451c-8a16-3212c4c03a95";

                var output = activities.Where(o => o.Id == GeneratedId).FirstOrDefault();

                repo.Setup(c => c.GetByIdAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()
                )).Returns(
                    Task.FromResult<Activity>(output)
                );

                var activityNew = new Activity()
                {
                    ActivityName = "The New Activity",
                    Description = "Activity Description"
                };

                var activityUpdated = new Activity()
                {
                    Id = GeneratedId,
                    ActivityName = "The New Activity",
                    Description = "Activity Description"
                };

                repo.Setup(c => c.UpdateAsync(
                    It.IsAny<string>(),
                    It.IsAny<Activity>(),
                    It.IsAny<EventGridOptions>(),
                    It.IsAny<string>()
                )).Returns(
                    Task.FromResult<Activity>(activityUpdated)
                );

                var svc = new ActivityService(repo.Object);

                // act
                var act = await svc.UpdateActivity(GeneratedId, activityNew);

                // assert
                Assert.Equal(activityUpdated.Id, act.Id);
                Assert.Equal(activityUpdated.ActivityName, act.ActivityName);
                Assert.Equal(activityUpdated.Description, act.Description);
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

                repo.Setup(c => c.DeleteAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<EventGridOptions>()
                ));

                var svc = new ActivityService(repo.Object);

                // act
                var act = await svc.DeleteActivity(GeneratedId);

                // assert
                Assert.Equal("Activity Deleted", act);
            }
        }
    }
}
