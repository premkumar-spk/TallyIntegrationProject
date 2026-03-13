namespace TallyIntegrationProject.Models
{
    public class SalesVoucher
    {
        public string? CompanyName { get; set; }

        public string? VoucherDate { get; set; }

        public string? CustomerLedger { get; set; }

        public string? ItemName { get; set; }

        public decimal Rate { get; set; }

        public decimal Quantity { get; set; }

        public decimal Amount => Rate * Quantity;
    }
}