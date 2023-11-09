using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using Xero.WebAapi.DataModels;
using Xero.WebAapi.IServices;
using Xero.WebAapi.Services;

namespace Xero.WebAapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoicesController : Controller
    {
        private readonly IWebHostEnvironment hostingEnvironment;        
        private readonly IQueueManager queueManager;
        private readonly ILogger<InvoicesController> logger;

        public InvoicesController(IWebHostEnvironment hostingEnvironment,         
            IQueueManager queueManager,
            ILogger<InvoicesController> logger)
        {
            this.hostingEnvironment = hostingEnvironment;           
            this.queueManager = queueManager;
            this.logger = logger;
        }        

        [HttpPost]
        
        public async Task<ActionResult<String>> Upload(IFormFile file,string email)
        {
            string fileName = "";
            try
            {                
                MailAddress mailAddress = new MailAddress(email);
                var extension = "." + file.FileName.Split('.')[file.FileName.Split(".").Length - 1];
                fileName = Guid.NewGuid() + "_" + DateTime.Now.Ticks.ToString() + extension;
                if(extension !=".pdf")
                    return BadRequest("pdf file is required for processing");
                var filePath = Path.Combine(hostingEnvironment.WebRootPath, "files");
                if(!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                var exactPath = Path.Combine(filePath, fileName);

                using (var stream = new FileStream(exactPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);                   
                }                

                InvoiceViewModel viewModel = new InvoiceViewModel();
                viewModel.Id = Guid.NewGuid().ToString();
                viewModel.UploadedBy = email;
                viewModel.UploadedDateTime = DateTime.Now;
                viewModel.Currency = Models.Currency.NONE;                               
                viewModel.FileName = exactPath;
                viewModel.FileSize = file.Length;
                viewModel.ProcessingStatus = Models.ProcessingStatus.PENDING;

                Task.Run(async () =>
                {
                    queueManager.AddAsync(viewModel);
                });

                return viewModel.Id;                           

            } 
            catch(Exception ex) {
                logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }            
            
        }
    }
}
