using System;

namespace API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            // ?? null coalescing operator
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        
        private string GetDefaultMessageForStatusCode(int statusCode)
        {
           return statusCode switch 
           {
               400 => "You made a bad request",
               401 => "Unauthorized",
               404 => "Resource not found",
               500 => "Server error",
               _ => null
           };
        }
    }
}