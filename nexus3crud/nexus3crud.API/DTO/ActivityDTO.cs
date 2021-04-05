using System;
using System.Collections.Generic;
using System.Text;

namespace nexus3crud.API.DTO
{
    public class ActivityDTO
    {
        // TODO: cek kembali apakah butuh [JsonProperty]
        // karena by default penamaan field tidak otomatis ter-serialize saat d baca dr api
        public string Id { get; set; }
        public string ActivityName { get; set; }
        public string Description { get; set; }
    }
}
