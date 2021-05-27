using Newtonsoft.Json;
using System.Collections.Generic;

namespace nexus3crud.BLL.DTO
{
    public class ActivityListDTO
    {
        [JsonProperty(propertyName: "continuationToken")]
        public string ContinuationToken { get; set; }
        [JsonProperty(propertyName: "items")]
        public List<ActivityDTO> Items { get; set; }
    }
}
