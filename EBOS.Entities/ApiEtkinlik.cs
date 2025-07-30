using Newtonsoft.Json;

namespace EBOS.Entities
{
    public class ApiEtkinlik
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string EtkinlikAdi { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("content")]
        public string Aciklama { get; set; }

        [JsonProperty("start")]
        public string Start { get; set; }

        [JsonProperty("end")]
        public string End { get; set; }

        [JsonProperty("poster_url")]
        public string GorselUrl { get; set; }

        [JsonProperty("venue")]
        public ApiMekan Venue { get; set; }  // NOT: Senin dosyada bu "ApiVenue" yazılmış ama senin entity klasöründe "ApiMekan.cs" var!

        [JsonProperty("category")]
        public ApiKategori Category { get; set; }
    }
}