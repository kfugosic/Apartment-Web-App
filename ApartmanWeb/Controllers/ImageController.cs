using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ImageResizer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApartmanWeb.Controllers
{
    public class ImageController : Controller
    {

        private readonly IHostingEnvironment _hostEnvironment;

        public ImageController(IHostingEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet("Get/{guid}")]
        public FileResult Get(string guid)
        {
            string rootPath = System.IO.Path.Combine(_hostEnvironment.WebRootPath, "images\\apartment");
            var path = Path.Combine(rootPath, guid + ".jpg");
            return new FileStreamResult(new FileStream(path, FileMode.Open), "image/jpeg");
        }

        [HttpGet("GetResized/{guid}")]
        public FileResult GetResized(string guid)
        {
            string rootPath = System.IO.Path.Combine(_hostEnvironment.WebRootPath, "images\\apartment");
            var path = Path.Combine(rootPath, guid + "tb.jpg");
            return new FileStreamResult(new FileStream(path, FileMode.Open), "image/jpeg");
        }

        [HttpGet("Delete/{guid}")]
        public IActionResult Delete(string guid)
        {
            string rootPath = System.IO.Path.Combine(_hostEnvironment.WebRootPath, "images\\apartment");
            var path = Path.Combine(rootPath, guid + ".jpg");
            var pathtb = Path.Combine(rootPath, guid + "tb.jpg");
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            if (System.IO.File.Exists(pathtb))
            {
                System.IO.File.Delete(pathtb);
            }
            return RedirectToAction("Images", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> FileUpload(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            Debug.WriteLine(_hostEnvironment.WebRootPath);
            string rootPath = System.IO.Path.Combine(_hostEnvironment.WebRootPath, "images\\apartment");
            Debug.WriteLine(rootPath);

            foreach (var formFile in files)
            {

                //string pic = System.IO.Path.GetFileName(formFile.FileName);
                int count = Directory.GetFiles(rootPath).Length / 2;
                string filePath = System.IO.Path.Combine(rootPath, count+".jpg");
                string thumbPath = System.IO.Path.Combine(rootPath, count + "tb.jpg");

                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    using (var stream = new FileStream(filePath, FileMode.Open))
                    using (var result = new FileStream(thumbPath, FileMode.Create))
                    {

                        var settings = new ResizeSettings
                        {
                            MaxWidth = 200,
                            MaxHeight = 200,
                            Format = "jpg"
                        };

                        ImageBuilder.Current.Build(stream, result, settings);
                        await formFile.CopyToAsync(result);

                    }
                }
            }

            //return Ok(new { count = files.Count, size, rootPath });
            return RedirectToAction("Images", "Admin");
        }

    }
}