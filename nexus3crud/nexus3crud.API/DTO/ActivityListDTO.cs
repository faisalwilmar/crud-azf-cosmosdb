using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace nexus3crud.API.DTO
{
    public class ActivityListDTO
    {
        [JsonProperty(propertyName: "continuationToken")]
        public string ContinuationToken { get; set; }
        [JsonProperty(propertyName: "items")]
        public List<ActivityDTO> Items { get; set; }
    }
}
