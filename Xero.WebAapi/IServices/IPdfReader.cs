namespace Xero.WebAapi.IServices
{
    public interface IPdfReader
    {
        Task PdfToText(string pdfFile);

    }
}
