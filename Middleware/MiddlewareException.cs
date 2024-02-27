using System.Net;

namespace NetKubernetes.Middleware;

public class MiddlewareException : Exception
{
    public HttpStatusCode Code { get; set; }
    public string Error { get; set; }

    public MiddlewareException(
        HttpStatusCode code,
        string error
    )
    {
        Code = code;
        Error = error;
    }
}