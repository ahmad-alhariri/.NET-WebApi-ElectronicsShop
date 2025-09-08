using System.Net;

namespace ElectronicsShop.Application.Common.Models;

public class ResponseHandler
{
    // 2xx Success Responses
    public GenericResponse<T> Success<T>(T data, string message = "Success", object meta = null)
    {
        return CreateResponse(HttpStatusCode.OK, true, data, message, meta);
    }

    public GenericResponse<T> Created<T>(T data, string message = "Created successfully", object meta = null)
    {
        return CreateResponse(HttpStatusCode.Created, true, data, message, meta);
    }

    public GenericResponse<T> Accepted<T>(T data = default, string message = "Request accepted", object meta = null)
    {
        return CreateResponse(HttpStatusCode.Accepted, true, data, message, meta);
    }

    public GenericResponse<T> NoContent<T>(string message = "No content")
    {
        return CreateResponse<T>(HttpStatusCode.NoContent, true, message: message);
    }

    // 3xx Redirection Responses
    public GenericResponse<T> MovedPermanently<T>(string location, string message = "Resource moved")
    {
        return CreateResponse<T>(HttpStatusCode.MovedPermanently, true, message: message,
            meta: new { Location = location });
    }

    public GenericResponse<T> NotModified<T>(string message = "Resource not modified")
    {
        return CreateResponse<T>(HttpStatusCode.NotModified, true, message: message);
    }

    // 4xx Client Errors
    public GenericResponse<T> BadRequest<T>(string message = "Bad request", IEnumerable<string> errors = null)
    {
        return CreateResponse<T>(HttpStatusCode.BadRequest, false, message: message, errors: errors);
    }

    public GenericResponse<T> Unauthorized<T>(string message = "Unauthorized")
    {
        return CreateResponse<T>(HttpStatusCode.Unauthorized, false, message: message);
    }

    public GenericResponse<T> Forbidden<T>(string message = "Forbidden")
    {
        return CreateResponse<T>(HttpStatusCode.Forbidden, false, message: message);
    }

    public GenericResponse<T> NotFound<T>(string message = "Resource not found")
    {
        return CreateResponse<T>(HttpStatusCode.NotFound, false, message: message);
    }

    public GenericResponse<T> MethodNotAllowed<T>(string message = "Method not allowed")
    {
        return CreateResponse<T>(HttpStatusCode.MethodNotAllowed, false, message: message);
    }

    public GenericResponse<T> Conflict<T>(string message = "Conflict occurred")
    {
        return CreateResponse<T>(HttpStatusCode.Conflict, false, message: message);
    }

    public GenericResponse<T> UnprocessableEntity<T>(string message = "Unprocessable entity",
        IEnumerable<string> errors = null)
    {
        return CreateResponse<T>(HttpStatusCode.UnprocessableEntity, false, message: message, errors: errors);
    }

    public GenericResponse<T> TooManyRequests<T>(string message = "Too many requests", TimeSpan? retryAfter = null)
    {
        return CreateResponse<T>(HttpStatusCode.TooManyRequests, false, message: message, meta: retryAfter != null
            ? new { RetryAfter = $"{retryAfter.Value.TotalSeconds} seconds" }
            : null);
    }

    // 5xx Server Errors
    public GenericResponse<T> InternalServerError<T>(string message = "Internal server error",
        IEnumerable<string> errors = null)
    {
        return CreateResponse<T>(HttpStatusCode.InternalServerError, false, message: message, errors: errors);
    }

    public GenericResponse<T> ServiceUnavailable<T>(string message = "Service unavailable", TimeSpan? retryAfter = null)
    {
        return CreateResponse<T>(HttpStatusCode.ServiceUnavailable, false, message: message, meta: retryAfter != null
            ? new { RetryAfter = $"{retryAfter.Value.TotalSeconds} seconds" }
            : null);
    }

    public GenericResponse<T> NotImplemented<T>(string message = "Feature not implemented")
    {
        return CreateResponse<T>(HttpStatusCode.NotImplemented, false, message: message);
    }

    // Specialized Responses
    public GenericResponse<T> Deleted<T>(string message = "Deleted successfully")
    {
        return CreateResponse<T>(HttpStatusCode.OK, true, message: message);
    }

    public GenericResponse<T> Paginated<T>(T items, int totalCount, int pageNumber, int pageSize, string message = "Success")
    {
        return CreateResponse(
            HttpStatusCode.OK,
            true,
            items,
            message,
            new
            {
                Pagination = new
                {
                     PageNumber = pageNumber,
                     PageSize = pageSize,
                     TotalCount = totalCount,
                     TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                     HasPreviousPage = pageNumber > 1,
                     HasNextPage = pageNumber < (int)Math.Ceiling(totalCount / (double)pageSize)
                     
                }
            }
        );
    }

    // Base factory method
    private GenericResponse<T> CreateResponse<T>(
        HttpStatusCode statusCode,
        bool succeeded,
        T data = default,
        string message = null,
        object meta = null,
        IEnumerable<string> errors = null)
    {
        return new GenericResponse<T>
        {
            StatusCode = statusCode,
            Succeeded = succeeded,
            Data = data,
            Message = message,
            Meta = meta,
            Errors = errors?.ToList() ?? new List<string>()
        };
    }
}