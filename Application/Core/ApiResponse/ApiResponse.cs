namespace API.Core.ApiResponse
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }

        public T Value { get; set; }

        public string Error { get; set; }

        public static ApiResponse<T> Success(T value) => new ApiResponse<T> { IsSuccess = true, Value = value };
        public static ApiResponse<T> Failure(string error) => new ApiResponse<T> { IsSuccess = false, Error = error };

    }
}
