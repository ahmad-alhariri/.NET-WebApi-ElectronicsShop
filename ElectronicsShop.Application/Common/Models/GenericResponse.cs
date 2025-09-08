using System.Net;

namespace ElectronicsShop.Application.Common.Models;


public class GenericResponse<T>
{
    // Success constructor
    public GenericResponse(T data, string message = null)
    {
        StatusCode = HttpStatusCode.OK;
        Succeeded = true;
        Data = data;
        Message = message ?? "Success";
    }

    // Error constructor
    public GenericResponse(HttpStatusCode statusCode, string message, bool succeeded = false)
    {
        StatusCode = statusCode;
        Succeeded = succeeded;
        Message = message;
    }

    // Generic error constructor
    public GenericResponse(string message, bool succeeded)
    {
        Succeeded = succeeded;
        Message = message;
    }

    public GenericResponse()
    {
    }

    public HttpStatusCode StatusCode { get; set; }
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public object Meta { get; set; }
    public List<string> Errors { get; set; } = new();
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}