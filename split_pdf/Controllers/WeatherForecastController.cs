using GrapeCity.Documents.Pdf;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Net;

namespace split_pdf.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public async Task<string> Get(string ppath)
        {
            GcPdfDocument doc = new GcPdfDocument();

            byte[] imageData = null;

            using (var httpClient = new HttpClient())
            {
                imageData = await httpClient.GetByteArrayAsync(ppath);
            }
            doc.Load(new MemoryStream(imageData));

            //Specify the options that should be used while saving the page to image 
            SaveAsImageOptions saveOptions = new SaveAsImageOptions()
            {
                BackColor = Color.White,
                DrawAnnotations = false,
                DrawFormFields = false,
                Resolution = 100
            };

            var list = new List<string>();


            foreach (var page in doc.Pages)
            {
                //Saves the document's page as an image to a stream in JPEG format
                MemoryStream stream = new MemoryStream();
                page.SaveAsPng(stream, saveOptions);

                string base64 = Convert.ToBase64String(stream.ToArray());
                list.Add(base64);
            }

            return string.Empty;
        }
    }
}