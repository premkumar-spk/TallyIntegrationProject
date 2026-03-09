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
            XmlGenerator xml = new XmlGenerator();

            var xmlData = xml.CreateLedgerXML(model.LedgerName, model.Parent);

            var result = await _service.SendToTally(xmlData);

            ViewBag.Result = result;

            return View();
        }

    }
}