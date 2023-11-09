namespace Xero.WebAapi.Models
{
    public class Invoice
    {        
        public string UploadedBy { get; set; }
        public DateTime UploadedDatestamp { get; set; }
        public long FileSize { get; set; }
        public string VendorName { get; set; }
        public DateTime? InvoiceDate { get; set; }

        public string TotalAmount { get; set; }
        public string TotalAmountDue { get; set; }
        public string Currency { get; set; }
        public string TaxAmount { get; set; }        
        public string ProcessingStatus { get; set; }
    }
}
