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
            XmlGenerator xml = new XmlGenerator();

            var xmlData = xml.CreateStockXML(model.Name, model.Unit);

            var result = await _service.SendToTally(xmlData);

            ViewBag.Result = result;

            return View();
        }

    }
}