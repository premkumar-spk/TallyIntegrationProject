using Microsoft.AspNetCore.Mvc;

namespace TallyIntegrationProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
