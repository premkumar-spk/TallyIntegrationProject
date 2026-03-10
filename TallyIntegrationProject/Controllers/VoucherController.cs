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

            // Null-check the model itself.
            if (model == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid request.");
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.CustomerName))
            {
                ModelState.AddModelError(nameof(model.CustomerName), "Customer name is required.");
            }

            if (string.IsNullOrWhiteSpace(model.ItemName))
            {
                ModelState.AddModelError(nameof(model.ItemName), "Item name is required.");
            }

            if (model.Amount <= 0)
            {
                ModelState.AddModelError(nameof(model.Amount), "Amount must be greater than zero.");
            }

            if (model.Date == default(DateTime))
            {
                ModelState.AddModelError(nameof(model.Date), "Voucher date is required.");
            }
            if (!ModelState.IsValid)
            {
                // Return the view with validation messages so the user can correct input.
                return View(model);
            }

            // At this point CustomerName and ItemName are non-null and non-whitespace.
            var customer = model.CustomerName!;
            var item = model.ItemName!;
            var amount = model.Amount;
            var date = model.Date;

            var tallyDate = date.ToString("yyyyMMdd");

            var xmlData = xml.CreateSalesVoucherXML(
                customer: customer,
                item: item,
                amount: amount,
                date: date,
                tallyDate: tallyDate
            );

            var result = await _service.SendToTally(xmlData);

            ViewBag.Result = result;

            return View();
        }

    }
}