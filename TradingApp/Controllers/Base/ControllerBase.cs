using System.Net;
using System.Runtime.CompilerServices;
using TradingApp.Models.Base;

namespace TradingApp.Controllers.Base;

public class ControllerBase
{
    public HttpListenerContext HttpContext { get; set; }

    public Result View([CallerMemberName] string MethodName = "")
    {
        using var writer = new StreamWriter(HttpContext.Response.OutputStream);

        var controllerName = this.GetType().Name[..this.GetType().Name.LastIndexOf("Controller")];

        var pageHtml = File.ReadAllText($"Views/{controllerName}/{MethodName}.html");
        writer.WriteLine(pageHtml);

        return new Result(HttpStatusCode.OK, "text/html");
    }

    public ObjectResult ObjectView(string page)
    {
        return new ObjectResult(page)
        {
            StatusCode = HttpStatusCode.OK,
        };
    }

    public Result Ok()
    {
        return new Result(HttpStatusCode.OK);
    }

    public ObjectResult Ok(object? model)
    {
        return new ObjectResult(model)
        {
            StatusCode = HttpStatusCode.OK,
        };
    }

    public Result Created()
    {
        return new Result(HttpStatusCode.Created);
    }

    public Result NotFound()
    {
        return new Result(HttpStatusCode.NotFound);
    }


    public ObjectResult BadRequest(object? model)
    {
        return new ObjectResult(model)
        {
            StatusCode = HttpStatusCode.BadRequest,
        };
    }
}