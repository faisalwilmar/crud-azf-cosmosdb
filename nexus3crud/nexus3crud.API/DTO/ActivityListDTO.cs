using System;
using System.Collections.Generic;
using System.Text;

namespace nexus3crud.API.DTO
{
    public class ActivityListDTO
    {
        public string ContinuationToken { get; set; }
        public List<ActivityDTO> Items { get; set; }
    }
}
