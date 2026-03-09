using Microsoft.AspNetCore.Mvc;
using TallyIntegrationProject.Models;

namespace TallyIntegrationProject.Controllers
{
    public class CompanyController : Controller
    {

        public IActionResult SelectCompany()
        {
            var companies = new List<TallyCompany>
            {
                new TallyCompany { Name="ABC Pvt Ltd"},
                new TallyCompany { Name="TechNova Solutions Pvt Ltd"}
            };

            return View(companies);
        }


        public IActionResult SetCompany(string companyName)
        {
            HttpContext.Session.SetString("TallyCompany", companyName);

            return RedirectToAction("Create", "Ledger");
        }

    }
}