using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Net.Http;

namespace SmartStudyPlanner.Controllers
{
    [Route("Ai")]
    public class AiController : Controller
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _http;

        public AiController(IConfiguration config, HttpClient http)
        {
            _config = config;
            _http = http;
        }

        // =========================
        // Open Page
        // =========================
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        // =========================
        // AI API
        // =========================
        [HttpPost("Task")]
        public async Task<IActionResult> Ask([FromBody] AiRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Question))
            {
                return Json(new { answer = "Please enter a question" });
            }

            var apiKey = _config["OpenRouter:ApiKey"];

            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var body = new
            {
                model = "openai/gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "user", content = request.Question }
                }
            };

            var response = await _http.PostAsJsonAsync(
                "https://openrouter.ai/api/v1/chat/completions",
                body
            );

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);

            var answer = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return Json(new { answer });
        }
    }

    public class AiRequest
    {
        public string Question { get; set; }
    }
}