using System.Text.Json;
using System.Net.Http.Headers;

namespace MindLink.Data.Services
{
    public class SentimentService
    {
        private readonly HttpClient _http;
        private const string ApiToken = "";
        private const string ModelUrl = "https://router.huggingface.co/hf-inference/pipeline/text-classification/distilbert-base-uncased-finetuned-sst-2-english";


        public SentimentService(HttpClient http) => _http = http;

        public async Task<string> AnalyzeAsync(string text)
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", ApiToken);

            var response = await _http.PostAsJsonAsync(ModelUrl, new { inputs = text });
            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine(response.StatusCode);

            Console.WriteLine("HuggingFaceResponse: " + json + "END HuggingFace");

            // Ако не е валиден JSON (моделът се зарежда) - връщаме neutral
            if (!json.TrimStart().StartsWith("["))
                return "neutral";

            using var doc = JsonDocument.Parse(json);
            var best = doc.RootElement[0]
                          .EnumerateArray()
                          .OrderByDescending(x => x.GetProperty("score").GetDouble())
                          .First();

            return best.GetProperty("label").GetString()?.ToLower() ?? "neutral";
        }
    }

}
