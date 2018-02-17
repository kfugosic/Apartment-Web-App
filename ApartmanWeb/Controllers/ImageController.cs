using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ApartmanWeb.Models;
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

 }
}