using System.Net;

namespace TradingApp.Models.Base;

public abstract class ActionResult
{
    public string? ContentType { get; set; }

    public HttpStatusCode StatusCode { get; set; }
}
