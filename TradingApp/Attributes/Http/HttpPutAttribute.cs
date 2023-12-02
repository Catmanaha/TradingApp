using AkhshamBazari.Attributes.Http.Base;

namespace AkhshamBazari.Attributes.Http;

public class HttpPutAttribute : HttpAttribute
{
    public HttpPutAttribute(string routing) : base(HttpMethod.Put, routing) { }

    public HttpPutAttribute() : base(HttpMethod.Put, null) { }
}
