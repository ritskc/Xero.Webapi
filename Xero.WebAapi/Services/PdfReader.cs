using System.Management.Automation;
using Xero.WebAapi.Controllers;
using Xero.WebAapi.IServices;

namespace Xero.WebAapi.Services
{
    public class PdfReader : IPdfReader
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly ILogger<PdfReader> logger;
        public PdfReader(IWebHostEnvironment hostingEnvironment,
            ILogger<PdfReader> logger)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
        }
        public async Task PdfToText(string pdfFile)
        {
            try
            {
                var inputPath = pdfFile;
                var outputPath = pdfFile.Replace(".pdf", ".txt");
                if (!File.Exists(outputPath))
                {
                    var txtFile = File.Create(outputPath);
                    txtFile.Close();
                }

                var toolPath = Path.Combine(hostingEnvironment.WebRootPath, "tool", "pdftotext.exe");
                using (PowerShell ps = PowerShell.Create())
                {
                    // specify the script code to run.
                    ps.AddScript($"{toolPath} -table {inputPath} {outputPath}");

                    // specify the parameters to pass into the script.
                    //ps.AddParameters(scriptParameters);

                    // execute the script and await the result.
                    var pipelineObjects = await ps.InvokeAsync().ConfigureAwait(false);

                    // print the resulting pipeline objects to the console.
                    foreach (var item in pipelineObjects)
                    {
                        Console.WriteLine(item.BaseObject.ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex.StackTrace);
            }
        }
    }
}
