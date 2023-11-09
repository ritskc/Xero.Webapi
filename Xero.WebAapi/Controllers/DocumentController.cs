using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xero.WebAapi.DataModels;
using Xero.WebAapi.IRepository;
using Xero.WebAapi.Models;

namespace Xero.WebAapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly ILogger<DocumentController> logger;
        private readonly IInvoiceRepository invoiceRepository;
        public DocumentController(IInvoiceRepository invoiceRepository,
            ILogger<DocumentController> logger) 
        {
            this.invoiceRepository = invoiceRepository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<Invoice>> Get(string id)
        {          

            try
            {
                var result= await invoiceRepository.GetAsync(id);
                Invoice invoice = new Invoice()
                {
                    UploadedBy = result.UploadedBy,
                    UploadedDatestamp = result.UploadedDateTime,
                    FileSize = result.FileSize,
                    VendorName = result.Vendor,
                    InvoiceDate = result.InvoiceDate,
                    TotalAmount = result.TotalAmount.ToString("0.00"),
                    TotalAmountDue = result.TotalAmountDue.ToString("0.00"),
                    Currency = result.Currency.ToString(),
                    TaxAmount = result.Tax.ToString("0.00"),
                    ProcessingStatus = result.ProcessingStatus.ToString(),
                };
                return invoice;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.StackTrace);
                return BadRequest();
            }
        }
    }
}
