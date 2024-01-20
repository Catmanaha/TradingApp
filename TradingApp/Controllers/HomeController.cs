using System.Net;
using TradingApp.Attributes.Http;
using TradingApp.Controllers.Base;
using TradingApp.Models.Base;

namespace TradingApp.Controllers;

public class HomeController : ControllerBase
{
    [HttpGet]
    public ActionResult Index()
    {
        return base.View();
    }
}