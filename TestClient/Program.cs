using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Press enter to start the test");
            Console.ReadLine();

            RunAsync().Wait();
        }

        public static async Task RunAsync()
        {
            var random = new Random();
            var languages = new string[] { "en-US", "es-ES", "fr-FR" };

            using (var client = new HttpClient())
            {
                var tests = Enumerable.Range(0, 5)
                    .Select(async (int number) => {
                        return await SendSingleTestRequestAsync(client, languages[random.Next(1000) % 3]);
                    });

                var results = await Task.WhenAll(tests);
                Debug.WriteLine(string.Join("\n", results));
            }
        }

        public static async Task<string> SendSingleTestRequestAsync(HttpClient client, string acceptLanguage)
        {
            using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri("http://localhost:60701/api/test")))
            {
                httpRequestMessage.Headers.Add("Accept-Language", acceptLanguage);
                var response = await client.SendAsync(httpRequestMessage);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return content;
                }
                else
                {
                    return string.Format("Response error: {0}", response.StatusCode);
                }
            }
        }
    }
}
