using System.Text;
using Xero.WebAapi.DataModels;
using Xero.WebAapi.IServices;

namespace Xero.WebAapi.Services
{
    public class TextFileParser : IFileParser
    {
        private readonly ILogger<TextFileParser> logger;
        public TextFileParser(ILogger<TextFileParser> logger)
        {
            this.logger = logger;
        }
        public async Task<InvoiceViewModel> GetFileData(string fileName, InvoiceViewModel invoiceViewModel)
        {
            try
            {
                const Int32 BufferSize = 128;
                using (var fileStream = File.OpenRead(fileName))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    String line;
                    bool ISNextLineCompany = false;
                    bool ISNextLineCurrency = false;
                    while ((line = await streamReader.ReadLineAsync()) != null)
                    {
                        line = line.Trim();
                        if (ISNextLineCompany && line!="")
                        {
                            invoiceViewModel.Vendor = Utilities.TextParser.GetCompanyName(line);
                            ISNextLineCompany = false;
                        }
                        DateTime invoiceDate;
                        if (Utilities.TextParser.TryGetInvoiceDate(line, out invoiceDate))
                        {
                            invoiceViewModel.InvoiceDate = invoiceDate;
                            ISNextLineCompany = true;
                        }

                        if (ISNextLineCurrency && line != "")
                        {
                            invoiceViewModel.Currency = Utilities.TextParser.TryGetCurrency(line);
                            ISNextLineCurrency = false;
                        }
                        string outputtype;
                        decimal amount;
                        if (Utilities.TextParser.TryGetRequiredData(line, out outputtype, out amount))
                        {
                            switch (outputtype)
                            {
                                case "Total":
                                    invoiceViewModel.TotalAmount = amount;
                                    break;
                                case "TotalDue":
                                    invoiceViewModel.TotalAmountDue = amount;
                                    ISNextLineCurrency = true;
                                    break;
                                case "Tax":
                                    invoiceViewModel.Tax = amount;
                                    break;
                            }
                        }               

                    }
                    invoiceViewModel.ProcessingStatus = Models.ProcessingStatus.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.StackTrace);
                invoiceViewModel.ProcessingStatus = Models.ProcessingStatus.FAIL;
            }

            return invoiceViewModel;
        }        
    }
}
