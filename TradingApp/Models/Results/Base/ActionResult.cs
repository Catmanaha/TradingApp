using System.Net;

namespace TradingApp.Models.Base;

public class ActionResult
{
    public string? ContentType { get; set; }

    public HttpStatusCode StatusCode { get; set; }
}
