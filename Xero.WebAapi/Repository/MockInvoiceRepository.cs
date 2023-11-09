using Xero.WebAapi.DataModels;
using Xero.WebAapi.IRepository;

namespace Xero.WebAapi.Repository
{
    public class MockInvoiceRepository : IInvoiceRepository
    {
        private List<InvoiceViewModel> invoices;

        public MockInvoiceRepository()
        {
            invoices = new List<InvoiceViewModel>();
        }
        public async Task<string> AddAsync(InvoiceViewModel invoice)
        {            
            await Task.Run(()=>invoices.Add(invoice));
            return invoice.Id;
        }

        public async Task<string> UpdateAsync(InvoiceViewModel invoice)
        {            
            await Task.Run(() =>
                { 
                    var oldOnvoice = invoices.Where(e=>e.Id == invoice.Id).FirstOrDefault();
                    oldOnvoice = invoice;
                }
            );
            return invoice.Id;
        }

        public async Task<InvoiceViewModel> GetAsync(string id)
        {            
           return await Task.Run(() => invoices.Where(e => e.Id == id).FirstOrDefault());            
        }
    }
}
