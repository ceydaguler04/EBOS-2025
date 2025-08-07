using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using EBOS.Entities;
using Newtonsoft.Json;

namespace EBOS.Helpers
{
    public static class EtkinlikApiYardimcisi
    {
        // Artık kategori int türünde
        public static async Task<List<ApiEtkinlik>> EtkinlikleriGetirAsync(int kategoriId)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Etkinlik-Token", "9adcf830cf7abf4e1c47e632aa006003");

            string url = "https://backend.etkinlik.io/api/v2/events?take=50";
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var wrapper = JsonConvert.DeserializeObject<ApiEtkinlikResponse>(json);
                return wrapper?.Items ?? new List<ApiEtkinlik>();
            }

            return new List<ApiEtkinlik>();
        }
        public static async Task<List<ApiEtkinlik>> TumEtkinlikleriGetirAsync()
        {
            var tumEtkinlikler = new List<ApiEtkinlik>();
            int skip = 0, take = 50;
            bool devam = true;

            while (devam)
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-Etkinlik-Token", "9adcf830cf7abf4e1c47e632aa006003");

                string url = $"https://backend.etkinlik.io/api/v2/events?skip={skip}&take={take}";
                Console.WriteLine($"API isteği: skip={skip}");  // log için

                try
                {
                    var response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode) break;

                    var json = await response.Content.ReadAsStringAsync();
                    var wrapper = JsonConvert.DeserializeObject<ApiEtkinlikResponse>(json);
                    var gelenEtkinlikler = wrapper?.Items ?? new List<ApiEtkinlik>();

                    if (gelenEtkinlikler.Count == 0)
                    {
                        devam = false;
                    }
                    else
                    {
                        tumEtkinlikler.AddRange(gelenEtkinlikler);
                        skip += take;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hata oluştu: " + ex.Message);
                    devam = false;
                }
            }

            return tumEtkinlikler;
        }

        //public static async Task<List<ApiEtkinlik>> TumEtkinlikleriGetirAsync()
        //{
        //    var tumEtkinlikler = new List<ApiEtkinlik>();
        //    int skip = 0, take = 50;
        //    bool devam = true;

        //    while (devam)
        //    {
        //        using var client = new HttpClient();
        //        client.DefaultRequestHeaders.Add("X-Etkinlik-Token", "9adcf830cf7abf4e1c47e632aa006003");

        //        string url = $"https://backend.etkinlik.io/api/v2/events?skip={skip}&take={take}";
        //        var response = await client.GetAsync(url);
        //        if (!response.IsSuccessStatusCode) break;

        //        var json = await response.Content.ReadAsStringAsync();
        //        var wrapper = JsonConvert.DeserializeObject<ApiEtkinlikResponse>(json);
        //        var liste = wrapper?.Items ?? new List<ApiEtkinlik>();

        //        if (liste.Count == 0)
        //            devam = false;
        //        else
        //        {
        //            tumEtkinlikler.AddRange(liste);
        //            skip += take;
        //        }
        //    }

        //    return tumEtkinlikler;
        //}
        public static async Task<ApiMekan?> MekanGetirAsync(int mekanId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-Etkinlik-Token", "9adcf830cf7abf4e1c47e632aa006003");
                string url = $"https://backend.etkinlik.io/api/v2/venues/{mekanId}";

                try
                {
                    var json = await client.GetStringAsync(url);
                    return JsonConvert.DeserializeObject<ApiMekan>(json);
                }
                catch
                {
                    return null;
                }
            }
        }

        public static async Task<List<ApiMekan>> MekanlariGetirAsync()
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Etkinlik-Token", "9adcf830cf7abf4e1c47e632aa006003");

            string url = "https://backend.etkinlik.io/api/v2/venues?take=1000";
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ApiMekanResponse>(json);
                return result?.Items ?? new List<ApiMekan>();
            }

            return new List<ApiMekan>();
        }
    }
}