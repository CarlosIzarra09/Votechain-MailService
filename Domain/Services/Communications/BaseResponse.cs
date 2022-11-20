namespace PRY20220278.Domain.Services.Communications
{
    public abstract class BaseResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get;  set; }
        public int StatusCode { get; set; }
        public T Resource { get; set; }

        public BaseResponse(T resource)
        {
            Resource = resource;
            Success = true;
            StatusCode = 202;
            Message = string.Empty;
        }

        public BaseResponse(string message, int statusCode)
        {
            StatusCode = statusCode;
            Success = false;
            Message = message;
        }
    }
}