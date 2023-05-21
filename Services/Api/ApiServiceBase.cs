using contact.Models.Api;
using contact.Models.Settings;
using contact.Utility;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace contact.Services.Api
{
    public abstract class ApiServiceBase
    {
        protected readonly ILogger logger;
        protected readonly ApiSettings ApiSettings;
        protected readonly HttpClient httpClient;

        public ApiServiceBase(
            ILogger logger,
            ApiSettings options)
        {
            this.logger = logger;
            ApiSettings = options;
            httpClient = new HttpClient();
        }

        public void SetAuthenticationHeaderBearer(string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        protected async Task<ApiResponse> GetAsync(string url)
        {
            return await GetResponseAsync(url);
        }

        protected async Task<ApiResponse<T>> GetAsync<T>(string url) where T : class, new()
        {
            try
            {
                return new ApiResponse<T>(await GetResponseAsync(url));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, LoggingMessage.GetMessage(LogLevel.Error), ex.Message);
                throw;
            }
        }

        protected async Task<ApiResponse<T>> PostAsync<T>(string url, List<KeyValuePair<string, string>> content = null) where T : class, new()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new FormUrlEncodedContent(content)
            };
            return await PostAsync<T>(request);
        }

        protected async Task<ApiResponse<T>> PostAsync<T>(string url, string jsonContent) where T : class, new()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };
            return await PostAsync<T>(request);
        }

        #region Private Methods

        private async Task<ApiResponse> GetResponseAsync(string url)
        {
            try
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse(await response.Content.ReadAsStringAsync());
                }

                logger.LogError(LoggingMessage.GetMessage(LogLevel.Error), response.ReasonPhrase);
                return new ApiResponse(response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, LoggingMessage.GetMessage(LogLevel.Error), ex.Message);
                throw;
            }
        }

        private async Task<ApiResponse<T>> PostAsync<T>(HttpRequestMessage requestMessage) where T : class, new()
        {
            try
            {
                var response = await httpClient.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<T>(JsonSerializer.Deserialize<T>(result));
                }

                logger.LogError(LoggingMessage.GetMessage(LogLevel.Error), response.ReasonPhrase);
                return new ApiResponse<T>(response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, LoggingMessage.GetMessage(LogLevel.Error), ex.Message);
                throw;
            }
        }
        #endregion
    }
}
