using Newtonsoft.Json;
using Nexus.Base.CosmosDBRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace nexus3crud.DAL.Model
{
    public class Activity : ModelBase
    {
        [JsonProperty(propertyName: "activityName")]
        public string ActivityName { get; set; }

        [JsonProperty(propertyName: "description")]
        public string Description { get; set; }
    }
}
