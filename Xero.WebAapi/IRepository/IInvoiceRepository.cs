using Xero.WebAapi.DataModels;

namespace Xero.WebAapi.IRepository
{
    public interface IInvoiceRepository
    {
        Task<string> AddAsync(InvoiceViewModel invoice);
        Task<string> UpdateAsync(InvoiceViewModel invoice);
        Task<InvoiceViewModel> GetAsync(string id);
    }
}
