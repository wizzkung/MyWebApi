using BmwFullProjectJquery.Model.DeepSeek;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BmwFullProjectJquery.Services
{
    public class DeepSeekChatService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://api.deepseek.com/v1/chat/completions";

        public DeepSeekChatService(string apiKey)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);
        }

        public async Task<ChatResponse> SendChatAsync(List<ChatMessage> messages,
                                                    CancellationToken cancellationToken = default)
        {
            try
            {
                var request = new ChatRequest
                {
                    messages = messages,
                    max_tokens = 1000
                };

                var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(ApiUrl, content, cancellationToken);
                var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    return new ChatResponse
                    {
                        error = new Error
                        {
                            message = $"HTTP Error: {response.StatusCode} - {responseJson}"
                        }
                    };
                }
                return JsonSerializer.Deserialize<ChatResponse>(responseJson)
                       ?? new ChatResponse { error = new Error { message = "Invalid response format" } };
            }
            catch (Exception ex)
            {
                return new ChatResponse
                {
                    error = new Error
                    {
                        message = $"Exception: {ex.Message}"
                    }
                };
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
