using System.ComponentModel.DataAnnotations;
using Xero.WebAapi.Models;

namespace Xero.WebAapi.DataModels
{
    public class InvoiceViewModel
    {
        public string Id { get; set; }
        public string UploadedBy { get; set; }
        public DateTime UploadedDateTime { get; set; }
        public string Vendor { get; set; }
        public DateTime? InvoiceDate { get; set; }
       
        public decimal TotalAmount { get; set; }
        public decimal TotalAmountDue { get; set; }
        public Currency Currency { get; set; }
        public decimal Tax { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public ProcessingStatus ProcessingStatus { get; set; }
    }
}
