using Newtonsoft.Json;

public class ApiMekan
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("slug")] public string Slug { get; set; }
    [JsonProperty("about")] public string About { get; set; }
    [JsonProperty("lat")] public string Lat { get; set; }
    [JsonProperty("lng")] public string Lng { get; set; }
    [JsonProperty("status")] public int Status { get; set; }
    [JsonProperty("phone")] public string Phone { get; set; }
    [JsonProperty("web_url")] public string WebUrl { get; set; }
    [JsonProperty("facebook_url")] public string FacebookUrl { get; set; }
    [JsonProperty("twitter_url")] public string TwitterUrl { get; set; }
    [JsonProperty("address")] public string Address { get; set; }
    [JsonProperty("city")] public ApiCity City { get; set; }
    [JsonProperty("district")] public ApiDistrict District { get; set; }
    [JsonProperty("neighborhood")] public ApiNeighborhood Neighborhood { get; set; }
}

public class ApiCity
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("slug")] public string Slug { get; set; }
}

public class ApiDistrict
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("slug")] public string Slug { get; set; }
}

public class ApiNeighborhood
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("slug")] public string Slug { get; set; }
}
