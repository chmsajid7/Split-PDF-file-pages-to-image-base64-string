using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;

namespace Free.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public async Task<string> ExtractPages(string path)
        {
            var reader = new PdfReader(path);
            var sourceDocument = new Document(reader.GetPageSizeWithRotation(1));

            using var ms = new MemoryStream();

            var pdfCopyProvider = new PdfCopy(sourceDocument, ms);

            sourceDocument.Open();

            for (int i = 1; i <= reader.NumberOfPages; i++)
            {

                var importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                pdfCopyProvider.AddPage(importedPage);

            }
            sourceDocument.Close();
            reader.Close();

            var pdf = ms.ToArray();

            var ac = Convert.ToBase64String(pdf);

            return null;
        }


        [HttpPost]
        public async Task<string> Get(string ppath)
        {
            string filepath = ppath;

            iTextSharp.text.pdf.PdfReader reader = null;
            int currentPage = 1;
            int pageCount = 0;

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

            reader = new iTextSharp.text.pdf.PdfReader(filepath);
            reader.RemoveUnusedObjects();
            pageCount = reader.NumberOfPages;
            string ext = System.IO.Path.GetExtension(filepath);
            for (int i = 1; i <= pageCount; i++)
            {
                iTextSharp.text.pdf.PdfReader reader1 = new iTextSharp.text.pdf.PdfReader(filepath);
                string outfile = filepath.Replace((System.IO.Path.GetFileName(filepath)), (System.IO.Path.GetFileName(filepath).Replace(".pdf", "") + "_" + i.ToString()) + ext);
                reader1.RemoveUnusedObjects();
                iTextSharp.text.Document doc = new iTextSharp.text.Document(reader.GetPageSizeWithRotation(currentPage));

                    var mem = new MemoryStream();

                iTextSharp.text.pdf.PdfCopy pdfCpy = new iTextSharp.text.pdf.PdfCopy(doc, new MemoryStream());
                doc.Open();
                for (int j = 1; j <= 1; j++)
                {
                    iTextSharp.text.pdf.PdfImportedPage page = pdfCpy.GetImportedPage(reader1, currentPage);


                    iTextSharp.text.pdf.PdfWriter wri = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, mem);

                    pdfCpy.AddPage(page);

                    var pdf = mem.ToArray();

                    var ac = Convert.ToBase64String(pdf);

                    currentPage += 1;
                }
                doc.Close();
                pdfCpy.Close();
                reader1.Close();
                reader.Close();
            }

            return "";
        }
    }
}