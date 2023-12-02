using AkhshamBazari.Attributes.Http.Base;

namespace AkhshamBazari.Attributes.Http;

public class HttpDeleteAttribute : HttpAttribute
{
    public HttpDeleteAttribute(string routing) : base(HttpMethod.Delete, routing) { }

    public HttpDeleteAttribute() : base(HttpMethod.Delete, null) { }
}
