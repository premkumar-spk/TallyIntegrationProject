using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
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
        public async Task<IActionResult> CreateSalesVoucher(SalesVoucher model)
        {
            XmlGenerator xml = new XmlGenerator();

            if (model == null)
            {
                ModelState.AddModelError("", "Invalid request.");
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.CustomerName))
                ModelState.AddModelError(nameof(model.CustomerName), "Customer name required");

            if (string.IsNullOrWhiteSpace(model.ItemName))
                ModelState.AddModelError(nameof(model.ItemName), "Item name required");

            if (model.Amount <= 0)
                ModelState.AddModelError(nameof(model.Amount), "Amount must be greater than zero");

            if (string.IsNullOrWhiteSpace(model.Date))
                ModelState.AddModelError(nameof(model.Date), "Voucher date required");

            if (!ModelState.IsValid)
                return View(model);

            var customer = model.CustomerName;
            var item = model.ItemName;
            var amount = model.Amount;

            // convert date to tally format
            var tallyDate = DateTime.ParseExact(model.Date, "yyyy-MM-dd", null).ToString("yyyyMMdd");

            var xmlData = xml.CreateSalesVoucherXML(
                customer,
                item,
                amount,
                tallyDate
            );

            var result = await _service.SendToTally(xmlData);

            // Parse the Tally response
            var doc = XDocument.Parse(result);

            var created = doc.Descendants("CREATED").FirstOrDefault()?.Value;

            // 👇 PLACE YOUR CODE HERE
            var lineError = doc.Descendants("LINEERROR").FirstOrDefault()?.Value;

            if (!string.IsNullOrEmpty(lineError))
            {
                ViewBag.Message = lineError;
            }
            else if (created == "1")
            {
                ViewBag.Message = "Voucher Created Successfully";
            }
            else
            {
                ViewBag.Message = "Voucher creation failed.";
            }

            ViewBag.TallyResponse = result;

            return View("Create", model);
        }

    }
}