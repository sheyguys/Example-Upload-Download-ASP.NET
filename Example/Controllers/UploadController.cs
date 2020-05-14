using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Example.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Example.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class UploadController : Controller
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _config;

        public UploadController(IHostingEnvironment env, IConfiguration config)
        {
            _env = env;
            _config = config;
        }

        [HttpPost("upload")]
        [AllowAnonymous]
        //public async Task<ActionResult> File([FromForm]FileModel f)
        public ActionResult Upload([FromForm]FileModel f)

        {
            // Getting Name
            string name = f.Name;
            // Getting Image
            var pdf = f.PDF;
            // Getting Image
            var xml = f.XML;
            // Saving Image on Server

            try
            {
                if (pdf.Length > 0 && xml.Length > 0)
                {
                    var pdfName = Path.GetFileName(pdf.FileName);
                    var pathPDF = Path.Combine(_env.ContentRootPath, "App_Data/PDF", pdfName);
                    using (var pdfstream = System.IO.File.Create(pathPDF))
                    {
                        pdf.CopyTo(pdfstream);
                    }
                    var xmlName = Path.GetFileName(xml.FileName);
                    var pathXML = Path.Combine(_env.ContentRootPath, "App_Data/XML", xmlName);
                    using (var xmlstream = System.IO.File.Create(pathXML))
                    {
                        xml.CopyTo(xmlstream);
                    }
                }
                return Ok(new { status = true, message = "File Upload Successfully" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }

        }

        [HttpGet("download/{fileName}")]
        [AllowAnonymous]
        public ActionResult Download(string fileName)
        {
            string path = Path.Combine(_env.ContentRootPath, "App_Data/PDF/", fileName);
            if (System.IO.File.Exists(path))
            {
                return File(path, "application/pdf");
            }
            return NotFound(new { status = false, message = "File NotFound" });
        }
    }
}
