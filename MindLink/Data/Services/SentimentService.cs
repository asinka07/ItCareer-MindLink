using System.Text.Json;
using VaderSharp2;

namespace MindLink.Data.Services
{
    public class SentimentService
    {
        private readonly HttpClient _http;

        public SentimentService(HttpClient http) => _http = http;

        public async Task<string> AnalyzeAsync(string text)
        {

            var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl=bg&tl=en&dt=t&q={Uri.EscapeDataString(text)}";
            var response = await _http.GetStringAsync(url);


            using var doc = JsonDocument.Parse(response);
            var translated = doc.RootElement[0][0][0].GetString() ?? text;

            var analyzer = new SentimentIntensityAnalyzer();
            var results = analyzer.PolarityScores(translated);

            if (results.Compound >= 0.05)
                return "positive";
            else if (results.Compound <= -0.05)
                return "negative";
            else
                return "neutral";
        }
    }
}