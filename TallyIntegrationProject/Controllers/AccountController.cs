using Microsoft.AspNetCore.Mvc;
using TallyIntegrationProject.Models;

namespace TallyIntegrationProject.Controllers
{
    public class AccountController : Controller
    {

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (model.Username == "admin" && model.Password == "123")
            {
                return RedirectToAction("SelectCompany", "Company");
            }

            return View();
        }

    }
}