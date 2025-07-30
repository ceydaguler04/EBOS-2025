using Newtonsoft.Json;
using System.Collections.Generic;

namespace EBOS.Entities
{
    public class ApiMekanResponse
    {
        [JsonProperty("meta")]
        public object Meta { get; set; }

        [JsonProperty("items")]
        public List<ApiMekan> Items { get; set; }
    }
}