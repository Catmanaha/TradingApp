using TradingApp.Attributes.Http.Base;

namespace TradingApp.Attributes.Http;

public class HttpPutAttribute : HttpAttribute
{
    public HttpPutAttribute(string routing) : base(HttpMethod.Put, routing) { }

    public HttpPutAttribute() : base(HttpMethod.Put, null) { }
}
