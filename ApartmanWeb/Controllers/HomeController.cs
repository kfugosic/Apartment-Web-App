using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApartmanWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Isam.Esent.Interop;

namespace ApartmanWeb.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHostingEnvironment _hostEnvironment;

        public HomeController(IHostingEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index2()
        {
            return View("Index");
        }

        public IActionResult Index()
        {
            string rootPath = System.IO.Path.Combine(_hostEnvironment.WebRootPath, "images\\apartment");
            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.ImageCounter = Directory.GetFiles(rootPath).Length / 2;
            homeViewModel.DirectReservation = false;
            String resultUrl = currentLanguageOrDefault() + "/Index";
            return View(resultUrl, homeViewModel);
        }

        [HttpGet("SetLanguage/{lang}")]
        public IActionResult SetLanguage(string lang)
        {
            if (lang.Equals("en") || lang.Equals("de") || lang.Equals("hr"))
            {
                HttpContext.Session.SetString("lang", lang);

            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Authorize]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string currentLanguageOrDefault()
        {
            var lang = HttpContext.Session.GetString("lang");
            if (lang == null)
            {
                lang = "hr";
            }
            return lang;
        }
    }
}
