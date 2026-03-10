using Microsoft.AspNetCore.Mvc;
using TallyIntegrationProject.Models;
using TallyIntegrationProject.Services;

namespace TallyIntegrationProject.Controllers
{
    public class StockController : Controller
    {

        private readonly TallyService _service;

        public StockController(TallyService service)
        {
            _service = service;
        }

        public IActionResult Create()
        {
            return View();
        }
     
        [HttpPost]
        public async Task<IActionResult> Create(StockItemModel model)
        {
            if (model == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid request.");
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError(nameof(model.Name), "Name is required.");
            }

            if (string.IsNullOrWhiteSpace(model.Unit))
            {
                ModelState.AddModelError(nameof(model.Unit), "Unit is required.");
            }

            if (string.IsNullOrWhiteSpace(model.Group))
            {
                ModelState.AddModelError(nameof(model.Group), "Group is required.");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            XmlGenerator xml = new XmlGenerator();

            // Safe to use null-forgiving operator because we've validated above.
            var xmlData = xml.CreateStockXML(model.Name!, model.Unit!,model.Group);

            var result = await _service.SendToTally(xmlData);

            ViewBag.Result = result;

            return View(model);
        }

    }
}