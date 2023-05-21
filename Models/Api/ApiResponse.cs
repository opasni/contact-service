namespace services.Models.Api
{
    using System.Net;
    using System.Text.Json;

    public class ApiResponse<T> : ApiResponse where T : class, new()
    {
        public T Data { get; set; }

        public ApiResponse(T data)
        {
            Data = data;
            IsSuccess = true;
        }

        public ApiResponse(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
            Data = new T();
        }

        public ApiResponse(ApiResponse response)
        {
            Message = response.Message;
            StatusCode = response.StatusCode;
            IsSuccess = response.IsSuccess;
            if (response.IsSuccess)
            {
                Data = JsonSerializer.Deserialize<T>(response.RawData);
            }
        }
    }

    public class ApiResponse
    {
        public string RawData { get; set; }

        public bool IsSuccess { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string Message { get; set; }

        public ApiResponse()
        { }

        public ApiResponse(string rawData)
        {
            RawData = rawData;
            IsSuccess = true;
        }

        public ApiResponse(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
            IsSuccess = false;
        }
    }
}