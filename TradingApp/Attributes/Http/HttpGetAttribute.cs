using TradingApp.Attributes.Http.Base;

namespace TradingApp.Attributes.Http;

public class HttpGetAttribute : HttpAttribute
{
    public HttpGetAttribute(string routing) : base(HttpMethod.Get, routing) { }

    public HttpGetAttribute() : base(HttpMethod.Get, null) { }
}
