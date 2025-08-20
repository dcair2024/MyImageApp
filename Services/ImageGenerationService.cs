
using System.Text;
using System.Text.Json;

namespace MyImageApp.Services
{
    public class ImageGenerationService : IImageGenerationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ImageGenerationService> _logger;

        public ImageGenerationService(HttpClient httpClient, IConfiguration configuration, ILogger<ImageGenerationService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> GenerateImageAsync(string prompt)
        {
            try
            {
                // Opção 1: OpenAI DALL-E
                return await GenerateWithOpenAI(prompt);

                // Opção 2: Stability AI
                // return await GenerateWithStabilityAI(prompt);

                // Opção 3: Hugging Face
                // return await GenerateWithHuggingFace(prompt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar imagem com prompt: {Prompt}", prompt);
                return null;
            }
        }

        private async Task<string> GenerateWithOpenAI(string prompt)
        {
            var apiKey = _configuration["OpenAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("OpenAI API Key não configurada");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            var requestBody = new
            {
                model = "dall-e-3",
                prompt = prompt,
                n = 1,
                size = "1024x1024",
                quality = "standard",
                response_format = "url"
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/images/generations", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OpenAIImageResponse>(responseContent);

                return result?.data?.FirstOrDefault()?.url;
            }

            var error = await response.Content.ReadAsStringAsync();
            _logger.LogError("Erro na API OpenAI: {StatusCode} - {Error}", response.StatusCode, error);
            return null;
        }

        private async Task<string> GenerateWithStabilityAI(string prompt)
        {
            var apiKey = _configuration["StabilityAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("Stability AI API Key não configurada");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            var requestBody = new
            {
                text_prompts = new[]
                {
                    new { text = prompt, weight = 1 }
                },
                cfg_scale = 7,
                height = 1024,
                width = 1024,
                samples = 1,
                steps = 30
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.stability.ai/v1/generation/stable-diffusion-xl-1024-v1-0/text-to-image", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<StabilityAIResponse>(responseContent);

                if (result?.artifacts?.Length > 0)
                {
                    // A Stability AI retorna base64, então você precisaria salvar e servir a imagem
                    var base64Data = result.artifacts[0].base64;
                    return await SaveBase64ImageAsync(base64Data);
                }
            }

            var error = await response.Content.ReadAsStringAsync();
            _logger.LogError("Erro na API Stability AI: {StatusCode} - {Error}", response.StatusCode, error);
            return null;
        }

        private async Task<string> GenerateWithHuggingFace(string prompt)
        {
            var apiKey = _configuration["HuggingFace:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("Hugging Face API Key não configurada");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            var requestBody = new { inputs = prompt };
            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Modelo gratuito popular no Hugging Face
            var response = await _httpClient.PostAsync("https://api-inference.huggingface.co/models/runwayml/stable-diffusion-v1-5", content);

            if (response.IsSuccessStatusCode)
            {
                var imageBytes = await response.Content.ReadAsByteArrayAsync();
                var base64Data = Convert.ToBase64String(imageBytes);
                return await SaveBase64ImageAsync(base64Data);
            }

            var error = await response.Content.ReadAsStringAsync();
            _logger.LogError("Erro na API Hugging Face: {StatusCode} - {Error}", response.StatusCode, error);
            return null;
        }

        private async Task<string> SaveBase64ImageAsync(string base64Data)
        {
            try
            {
                var imageBytes = Convert.FromBase64String(base64Data);
                var fileName = $"generated_{Guid.NewGuid()}.png";
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "generated");

                Directory.CreateDirectory(uploadsPath);

                var filePath = Path.Combine(uploadsPath, fileName);
                await File.WriteAllBytesAsync(filePath, imageBytes);

                return $"/images/generated/{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar imagem base64");
                return null;
            }
        }

        public async Task<bool> IsServiceAvailableAsync()
        {
            try
            {
                // Teste simples de conectividade
                var response = await _httpClient.GetAsync("https://api.openai.com/v1/models");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }

    // DTOs para as respostas das APIs
    public class OpenAIImageResponse
    {
        public OpenAIImageData[] data { get; set; }
    }

    public class OpenAIImageData
    {
        public string url { get; set; }
    }

    public class StabilityAIResponse
    {
        public StabilityAIArtifact[] artifacts { get; set; }
    }

    public class StabilityAIArtifact
    {
        public string base64 { get; set; }
        public int seed { get; set; }
        public string finishReason { get; set; }
    }
}