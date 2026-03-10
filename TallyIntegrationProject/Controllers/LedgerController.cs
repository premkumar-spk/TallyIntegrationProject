using Microsoft.AspNetCore.Mvc;
using TallyIntegrationProject.Models;
using TallyIntegrationProject.Services;

namespace TallyIntegrationProject.Controllers
{
    public class LedgerController : Controller
    {

        private readonly TallyService _service;

        public LedgerController(TallyService service)
        {
            _service = service;
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(LedgerModel model)
        {
            // Copy properties to locals for reliable null-checking
            var ledgerName = model?.LedgerName;
            var parent = model?.Parent;

            // Validate required inputs
            if (string.IsNullOrWhiteSpace(ledgerName))
            {
                ModelState.AddModelError(nameof(model.LedgerName), "Ledger name is required.");
            }

            if (string.IsNullOrWhiteSpace(parent))
            {
                ModelState.AddModelError(nameof(model.Parent), "Parent is required.");
            }
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                XmlGenerator xml = new XmlGenerator();

                // Locals are validated; use null-forgiving operator to satisfy the compiler if necessary
                var xmlData = xml.CreateLedgerXML(ledgerName!, parent!);

                var result = await _service.SendToTally(xmlData);

                //TempData["Result"] = result;
                //return RedirectToAction("Create", "Stock");

                if (result.Contains("<CREATED>1") || result.Contains("<ALTERED>1"))
                {
                    return RedirectToAction("Create", "Stock", new { ledger = ledgerName });
                }
                ViewBag.Result = result;
            }
            catch (Exception ex)
            {
                ViewBag.Result = ex.Message;
            }


            return View(model);
        }
    }
}