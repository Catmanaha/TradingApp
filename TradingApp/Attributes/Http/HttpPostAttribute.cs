using AkhshamBazari.Attributes.Http.Base;

namespace AkhshamBazari.Attributes.Http;

public class HttpPostAttribute : HttpAttribute
{
    public HttpPostAttribute(string routing) : base(HttpMethod.Post, routing) { }

    public HttpPostAttribute() : base(HttpMethod.Post, null) { }
}
