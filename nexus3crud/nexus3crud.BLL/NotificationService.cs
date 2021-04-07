using nexus3crud.DAL.Model;
using nexus3crud.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace nexus3crud.BLL
{
    public class NotificationService
    {
        public async void CreateNewNotification(string messageBody)
        {
            var actNot = new NotificationActivity()
            {
                Id = Guid.NewGuid().ToString(),
                MessageBody = messageBody,
                UtcTime = DateTime.UtcNow
            };
            using (var reps = new Repositories.NotificationActivityRepository("Course"))
            {
                await reps.CreateAsync(actNot);
            }
        }
    }
}
