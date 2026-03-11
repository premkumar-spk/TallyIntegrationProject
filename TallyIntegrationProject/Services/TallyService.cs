using System.Text;

namespace TallyIntegrationProject.Services
{
    public class TallyService
    {
        private readonly HttpClient _httpClient;

        public TallyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> SendToTally(string xmlData)
        {
            using var client = new HttpClient();

            var content = new StringContent(xmlData, Encoding.UTF8, "text/xml");

            var response = await client.PostAsync("http://localhost:9000", content);

            var result = await response.Content.ReadAsStringAsync();

            return result;
        }
    }
}