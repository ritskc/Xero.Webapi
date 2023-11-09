using System.Reflection.PortableExecutable;
using Xero.WebAapi.Controllers;
using Xero.WebAapi.DataModels;
using Xero.WebAapi.IRepository;
using Xero.WebAapi.IServices;

namespace Xero.WebAapi.Services
{
    public class QueueManager : IQueueManager
    {
        private readonly ILogger<QueueManager> logger;
        private readonly IFileParser fileParser;
        private readonly IInvoiceRepository invoiceRepository;
        private readonly IPdfReader pdfReader;
        private Queue<InvoiceViewModel> invoices;
        public QueueManager(IFileParser fileParser,
            IPdfReader pdfReader,
            IInvoiceRepository invoiceRepository,
            ILogger<QueueManager> logger)
        {
            this.fileParser = fileParser;
            this.pdfReader = pdfReader;
            this.invoiceRepository = invoiceRepository;
            this.logger = logger;
            invoices = new Queue<InvoiceViewModel>();
        }
        public async Task AddAsync(InvoiceViewModel invoice)
        {            
            await Task.Run(() => { 
                invoices.Enqueue(invoice);
                invoiceRepository.AddAsync(invoice);               
            });

            Task task =Task.Run(()=>ProcessAsync());
        }

        public async Task ProcessAsync()
        {
            while(invoices.Count > 0)
            {
                var  result =invoices.Dequeue();
                if (result != null)
                {
                    try
                    {
                        await pdfReader.PdfToText(result.FileName);
                        var data = await fileParser.GetFileData(result.FileName.Replace(".pdf", ".txt"), result);
                        await invoiceRepository.UpdateAsync(data);
                        File.Delete(result.FileName);
                        File.Delete(result.FileName.Replace(".pdf", ".txt"));
                    }
                    catch(Exception ex)
                    {
                        logger.LogError(ex.StackTrace);
                        throw new Exception("Error processing queue");
                    }
                                
                }
            }           
        }
    }
}
