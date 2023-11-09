using Xero.WebAapi.DataModels;

namespace Xero.WebAapi.IServices
{
    public interface IQueueManager
    {
        Task AddAsync(InvoiceViewModel model);
        Task ProcessAsync();
    }
}
