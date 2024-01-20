using System.Net;

namespace TradingApp.Models.Base;

public class Result : ActionResult
{
    public Result(HttpStatusCode statusCode, string? contentType = null)
    {
        base.StatusCode = statusCode;
        base.ContentType = contentType;
    }

}
