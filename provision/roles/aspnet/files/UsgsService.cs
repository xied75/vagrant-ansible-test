namespace aspnetcoreapp
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Runtime.Serialization.Json;
    using System.Threading.Tasks;

    public class UsgsService
    {
        static string baseUrl = "http://earthquake.usgs.gov/fdsnws/event/1/";

        public static async Task<RootObject> MakeRequestAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    using (HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get,
                        string.Format("query?format=geojson&starttime={0}&endtime={1}&minmagnitude=4",
                        DateTime.UtcNow.AddDays(-1.0).ToString("yyyy-MM-dd"),
                        DateTime.UtcNow.ToString("yyyy-MM-dd"))))
                    {
                        var response = await client.SendAsync(req).ConfigureAwait(false);
                        using (Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        {
                            DataContractJsonSerializer jsonSerializer =
                                new DataContractJsonSerializer(typeof(RootObject));
                            object objResponse = jsonSerializer.ReadObject(stream);
                            RootObject jsonResponse = objResponse as RootObject;
                            return jsonResponse;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
