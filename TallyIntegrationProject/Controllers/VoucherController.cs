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

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> CreateSalesVoucher(SalesVoucher model)
        {
            if (!ModelState.IsValid || model == null) { return View("Create", model); }

            XmlGenerator xmlGen = new XmlGenerator();
            //1.Check if Customer exists
            if (!await MasterExistsInTally(model.CustomerLedger!, "Ledger"))
            {
                ViewBag.Message = $"Error: The Ledger '{model.CustomerLedger}' does not exist in Tally. Please create it first.";
                return View("Create", model);
            }

            // 2. Check if Stock Item exists
            if (!await MasterExistsInTally(model.ItemName!, "StockItem"))
            {
                ViewBag.Message = $"Error: The Stock Item '{model.ItemName}' does not exist in Tally.";
                return View("Create", model);
            }

            bool partyExists = await MasterExistsInTally(model.CustomerLedger!, "Ledger");

            if (!partyExists)
            {
                // Auto-create ledger
                string createLedgerXml = xmlGen.CreateLedgerXML(
                    company: model.CompanyName ?? "" ,
                    ledgerName: model.CustomerLedger!,
                    parentGroup: "Sundry Debtors"   // ← very important
                );

                string createResponse = await _service.SendToTally(createLedgerXml);
            }
                string VoucherDate = "";

                if (DateTime.TryParseExact(model.VoucherDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                {
                    VoucherDate = parsedDate.ToString("yyyyMMdd");
                }
                else
                {
                    ViewBag.Message = "Invalid Date Format. Please use YYYY-MM-DD.";
                    return View("Create", model);
                }

                var xmlData = xmlGen.CreateSalesVoucherXML(
                    companyname: model.CompanyName ?? "",
                    voucherdate: VoucherDate,
                    customerledger: model.CustomerLedger ?? "",
                    itemName: model.ItemName ?? "",
                    quantity: model.Quantity ,
                    rate: model.Rate,
                    amount: model.Amount 
                );

                var result = await _service.SendToTally(xmlData);

            string folderPath = @"C:\Users\Public\TallyResponse";
            string filePath = Path.Combine(folderPath, "TallyXMLResponse.txt");

            // create folder if it does not exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // write response
            System.IO.File.WriteAllText(filePath, result);

            if (string.IsNullOrEmpty(result))
            {
                System.IO.File.WriteAllText(@"C:\Users\Public\TallyResponse\TallyXMLResponse.txt", "Result is empty");
            }
            else
            {
                System.IO.File.WriteAllText(@"C:\Users\Public\TallyResponse\TallyXMLResponse.txt", result);
            }

            if (string.IsNullOrWhiteSpace(result))
            {
                ViewBag.Message = "Empty response from Tally.";
                return View("Create", model);
            }

            try
            {
                var doc = XDocument.Parse(result);

                var created = doc.Descendants("CREATED").FirstOrDefault()?.Value;
                var lineError = doc.Descendants("LINEERROR").FirstOrDefault()?.Value;

                if (created == "1")
                    ViewBag.Message = "Voucher Created Successfully!";
                else
                    ViewBag.Message = lineError ?? "Voucher creation failed.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Invalid XML returned from Tally.";
            }

            ViewBag.TallyResponse = result;
            return View("Create", model);
        }
        private async Task<bool> MasterExistsInTally(string name, string type)
        {
            XmlGenerator xmlGen = new XmlGenerator();
            string checkXml = xmlGen.CheckMasterExistsXML(name, type);
            try
            {
                string response = await _service.SendToTally(checkXml);
                var doc = XDocument.Parse(response);

                // Tally returns the object tag (LEDGER or STOCKITEM) if it exists
                return doc.Descendants(type.ToUpper()).Any();
            }
            catch
            {
                return false;
            }
        }
    }
}