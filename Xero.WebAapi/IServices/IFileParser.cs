using Xero.WebAapi.DataModels;

namespace Xero.WebAapi.IServices
{
    public interface IFileParser
    {
        Task<InvoiceViewModel> GetFileData(string filePath,InvoiceViewModel invoiceViewModel);
    }
}
