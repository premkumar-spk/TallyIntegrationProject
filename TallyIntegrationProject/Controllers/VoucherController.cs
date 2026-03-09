using Microsoft.AspNetCore.Mvc;
using TallyIntegrationProject.Models;
using TallyIntegrationProject.Services;

namespace TallyIntegrationProject.Controllers
{
    public class VoucherController : Controller
    {

        private readonly TallyService _service;

        public VoucherController(TallyService service)
        {
            _service = service;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SalesVoucher model)
        {
            XmlGenerator xml = new XmlGenerator();

            var xmlData = xml.CreateSalesVoucherXML(model.CustomerName, model.ItemName, model.Amount);

            var result = await _service.SendToTally(xmlData);

            ViewBag.Result = result;

            return View();
        }

    }
}