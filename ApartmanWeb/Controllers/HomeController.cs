using System;
using System.Collections.Generic;
using System.Diagnostics;
using ApartmanWeb.Data;
using Microsoft.AspNetCore.Mvc;
using ApartmanWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ApartmanWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostEnvironment;
        private IApplicationSettingsRepository _appSettingsRepository;
        private IGuestReviewsRepository _guestReviewsRepository;
        private IConfiguration _configuration;

        public HomeController(IHostingEnvironment hostEnvironment,
            IApplicationSettingsRepository appSettingsRepository,
            IGuestReviewsRepository guestReviewsRepository,
            IConfiguration configuration)
        {
            _hostEnvironment = hostEnvironment;
            _appSettingsRepository = appSettingsRepository;
            _guestReviewsRepository = guestReviewsRepository;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            HomeViewModel homeViewModel = generateHomeViewModel();
            homeViewModel.ReviewsList = _guestReviewsRepository.getApprovedAndWithPermission();
            String resultUrl = currentLanguageOrDefault() + "/Index";
            Debug.WriteLine(resultUrl);
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        private string currentLanguageOrDefault()
        {
            var lang = HttpContext.Session.GetString("lang");
            if (lang == null)
            {
                lang = _configuration["AppSettings:DefaultLanguage"];
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

            homeViewModel.ImageOrder = imagesOrderList;
            return homeViewModel;
        }
    }
}