using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class TallyService
{
    private readonly HttpClient _httpClient;

    public TallyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> SendToTally(string xmlData)
    {
        try
        {
            var content = new StringContent(xmlData ?? string.Empty, Encoding.UTF8, "application/xml");
            using var response = await _httpClient.PostAsync("http://localhost:9000", content);

            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // Return status + body so caller can diagnose; never return null.
                return $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}: {result ?? string.Empty}";
            }

            return result ?? string.Empty;
        }
        catch (Exception ex)
        {
            // Consider using a logger; return non-null error text for debugging.
            return $"ERROR: {ex.GetType().Name}: {ex.Message}";
        }
    }
}

public class VoucherController
{
    private readonly TallyService _service;

    public VoucherController(TallyService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Create(Model model)
    {
        // ... other code ...

        var result = await _service.SendToTally(xmlData);
        result ??= string.Empty; // ensure not null

        string folderPath = @"C:\Users\Public\TallyResponse";
        string filePath = Path.Combine(folderPath, "TallyXMLResponse.txt");

        // create folder if it does not exist
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // write response safely
        if (string.IsNullOrEmpty(result))
        {
            System.IO.File.WriteAllText(filePath, "Result is empty");
            ViewBag.Message = "Empty response from Tally.";
            return View("Create", model);
        }
        else
        {
            System.IO.File.WriteAllText(filePath, result);
        }

        // ... other code ...

        return View("Create", model);
    }
}