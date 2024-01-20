using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using TradingApp.Attributes.Http.Base;
using TradingApp.Controllers;
using TradingApp.Controllers.Base;
using TradingApp.Models.Base;

HttpListener httpListener = new HttpListener();

const int port = 8080;
httpListener.Prefixes.Add($"http://*:{port}/");

httpListener.Start();

void AddValuesForResult(HttpListenerContext context, Result result)
{
    context.Response.ContentType = result.ContentType;
    context.Response.StatusCode = (int)result.StatusCode;
}

async Task AddValuesForObjectResult(HttpListenerContext context, ObjectResult objectResult)
{
    using var writer = new StreamWriter(context.Response.OutputStream);

    context.Response.ContentType = objectResult.ContentType;
    context.Response.StatusCode = (int)objectResult.StatusCode;
    await writer.WriteLineAsync(objectResult.Body);

}


while (true)
{
    var context = await httpListener.GetContextAsync();

    var endpointItems = context.Request.Url?.AbsolutePath?.Split("/", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    if (endpointItems == null || endpointItems.Any() == false)
    {
        new HomeController
        {
            HttpContext = context
        }.Index();
        context.Response.Close();
        continue;
    }

    var controllerType = Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(t => t.BaseType == typeof(ControllerBase))
        .FirstOrDefault(t => t.Name.ToLower() == $"{endpointItems[0]}controller");

    if (controllerType == null)
    {
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        context.Response.Close();
        continue;
    }

    string normalizedRequestHttpMethod = context.Request.HttpMethod.ToLower();

    var controllerMethod = controllerType
        .GetMethods()
        .FirstOrDefault(m =>
        {
            return m.GetCustomAttributes()
                .Any(attr =>
                {
                    if (attr is HttpAttribute httpAttribute)
                    {
                        bool isHttpMethodCorrect = httpAttribute.MethodType.Method.ToLower() == normalizedRequestHttpMethod;

                        if (isHttpMethodCorrect)
                            if (endpointItems.Length == 1 && httpAttribute.NormalizedRouting == null)
                            {
                                return true;
                            }

                            else if (endpointItems.Length > 1)
                            {
                                if (httpAttribute.NormalizedRouting == null)
                                {
                                    return false;

                                }
                                else
                                {
                                    var expectedEndpoint = string.Join('/', endpointItems[1..]).ToLower();
                                    var actualEndpoint = httpAttribute.NormalizedRouting;
                                    return actualEndpoint == expectedEndpoint;
                                }
                            }
                    }

                    return false;
                });
        });

    if (controllerMethod == null)
    {
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        context.Response.Close();
        continue;
    }

    var controller = Activator.CreateInstance(controllerType) as ControllerBase;

    controller.HttpContext = context;

    var methodCall = controllerMethod.Invoke(controller, null);

    if (methodCall is Task<ActionResult> methodCallAsActionResultTask)
    {

        if (methodCallAsActionResultTask.Result is ObjectResult methodCallAsObjectResultTask)
        {
            await AddValuesForObjectResult(context, methodCallAsObjectResultTask);
        }
        else if (methodCallAsActionResultTask.Result is Result methodCallAsResultTask)
        {
            AddValuesForResult(context, methodCallAsResultTask);
        }


    }
    else if (methodCall is ActionResult methodCallAsActionResult)
    {
        if (methodCallAsActionResult is ObjectResult methodCallAsObjectResult)
        {
            await AddValuesForObjectResult(context, methodCallAsObjectResult);

        }
        else if (methodCallAsActionResult is Result methodCallAsResult)
        {
            AddValuesForResult(context, methodCallAsResult);
        }

    }



    if (methodCall != null && methodCall is Task asyncMethod)
    {
        await asyncMethod.WaitAsync(CancellationToken.None);
    }

    context.Response.Close();
}