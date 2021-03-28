using Moq;
using Nexus.Base.CosmosDBRepository;
using nexuscrud.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace nexuscrud.BLL.Test
{
    public class ActivityServiceTest
    {
        public class GetActivityById
        {
            [Theory]
            [InlineData("354806d7-a5b7-451c-8a16-3212c4c03a95")]
            [InlineData("354806d7-a5b7-451c-8a16-3212c4c03xxx")]
            public async Task GetActivityById_ResultFound(string id)
            {
                // arrange
                var repo = new Mock<IDocumentDBRepository<Activity>>();

                IEnumerable<Activity> activities = new List<Activity>
                {
                    {new Activity() { Id = "354806d7-a5b7-451c-8a16-3212c4c03a95", ActivityName = "Activity to String", Description = "Description of Activity for delete"} },
                    {new Activity() { Id = "cf2f68d1-361f-4b3e-944b-d0d5806af395", ActivityName = "Activity to String to del", Description = "Description of Activity for delete"} },
                    {new Activity() { Id = "22a7f1e3-6783-47ea-b499-43eeb8569a32", ActivityName = "Activity to String to del XXX", Description = "Description of Activity for delete"} }
                };

                var activityData = activities.Where(o => o.Id == id).FirstOrDefault();

                repo.Setup(c => c.GetByIdAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()
                )).Returns(
                    Task.FromResult<Activity>(activityData)
                );

                var svc = new ActivityService(repo.Object);

                // act
                var act = await svc.GetActivityById(id);

                // assert
                Assert.Equal(activityData, act);
            }
        }
    }
}
