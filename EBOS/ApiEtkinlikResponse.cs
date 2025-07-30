using Newtonsoft.Json;
using System.Collections.Generic;

namespace EBOS.Entities
{
    public class ApiEtkinlikResponse
    {
        [JsonProperty("meta")]
        public object Meta { get; set; }

        [JsonProperty("items")]
        public List<ApiEtkinlik> Items { get; set; }
    }
}