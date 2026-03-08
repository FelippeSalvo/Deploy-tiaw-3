using Microsoft.AspNetCore.Mvc;

namespace PCraft.Core.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("Hello World - Começo da Sprint 1!", "text/plain");
        }
    }
}

