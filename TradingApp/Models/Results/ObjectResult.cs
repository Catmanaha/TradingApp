using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace TradingApp.Models.Base;

public class ObjectResult : ActionResult
{
    public string? Body { get; set; }

    public ObjectResult(object? model)
    {

        if (model is null)
        {
            base.StatusCode = HttpStatusCode.NoContent;
            return;
        }

        if (model is string modelString)
        {
            var regex = new Regex(@"<[^>]+>");
            var check = regex.IsMatch(modelString);

            if (check)
            {
                base.ContentType = "text/html";
                this.Body = modelString;
                return;
            }
        }

        if (model.GetType().IsValueType)
        {

            base.ContentType = "text/plain";
            this.Body = model.ToString()!;
        }
        else
        {
            base.ContentType = "application/json";
            this.Body = JsonSerializer.Serialize(model);
        }
    }
}
