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

        public async Task<string> SendToTally(string xml)
        {
            var content = new StringContent(xml, Encoding.UTF8, "application/xml");

            var response = await _httpClient.PostAsync("http://localhost:9000", content);

            return await response.Content.ReadAsStringAsync();
        }
    }
}