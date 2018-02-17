using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApartmanWeb.Data;
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
        private IApplicationSettingsRepository _appSettingsRepository;

        public HomeController(IHostingEnvironment hostEnvironment, IApplicationSettingsRepository appSettingsRepository)
        {
            _hostEnvironment = hostEnvironment;
            _appSettingsRepository = appSettingsRepository;
        }

        public IActionResult Index2()
        {
            return View("Index");
        }

        public IActionResult Index()
        {
            HomeViewModel homeViewModel = generateHomeViewModel();

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

        private HomeViewModel generateHomeViewModel()
        {
            string rootPath = System.IO.Path.Combine(_hostEnvironment.WebRootPath, "images\\apartment");
            HomeViewModel homeViewModel = new HomeViewModel();
            var appSettings = _appSettingsRepository.Get();
            homeViewModel.DirectReservation = appSettings.DirectReservation;
            var imagesOrder = appSettings.Order;
            var imageIds = imagesOrder.Split('-');
            List<int> imagesOrderList = new List<int>();
            foreach (var id in imageIds)
            {
                if (!String.IsNullOrEmpty(id))
                {
                    imagesOrderList.Add(int.Parse(id));
                }
            }
            homeViewModel.imageOrder = imagesOrderList;
            return homeViewModel;
        }
    }
}
