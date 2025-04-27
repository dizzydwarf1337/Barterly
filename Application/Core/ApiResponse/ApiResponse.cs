namespace API.Core.ApiResponse
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }

        public T Value { get; set; }

        public string Error { get; set; }
        
        public int StatusCode { get; set; }

        public static ApiResponse<T> Success(T value, int statusCode = 200) => new ApiResponse<T> { IsSuccess = true, Value = value, StatusCode = statusCode };
        public static ApiResponse<T> Failure(string error, int statusCode = 400) => new ApiResponse<T> { IsSuccess = false, Error = error, StatusCode = statusCode };

    }
}
