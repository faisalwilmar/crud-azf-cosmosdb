using Newtonsoft.Json;
using Nexus.Base.CosmosDBRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace nexus3crud.DAL.Model
{
    public class NotificationActivity : ModelBase
    {
        [JsonProperty(propertyName: "id")]
        public string Id { get; set; }

        [JsonProperty(propertyName: "messageBody")]
        public string MessageBody { get; set; }

        [JsonProperty(propertyName: "utcTime")]
        public DateTime UtcTime { get; set; }
    }
}
