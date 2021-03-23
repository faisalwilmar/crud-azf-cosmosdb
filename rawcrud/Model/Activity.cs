using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace rawcrud.Model
{
    public class Activity
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("activityName")]
        public string ActivityName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("partitionKey")]
        public string PartitionKey { get; set; }
    }
}
