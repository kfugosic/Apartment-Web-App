using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApartmanWeb.Data;
using ApartmanWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ApartmanWeb.Controllers
{

    [Authorize]
    public class GuestsPageController : Controller
    {

        private readonly IHostingEnvironment _hostEnvironment;
        private IGuestReviewsRepository _guestReviewsRepository;
        private IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;


        public GuestsPageController(IHostingEnvironment hostEnvironment, 
                                    IGuestReviewsRepository guestReviewsRepository, 
                                    IConfiguration configuration, 
                                    UserManager<ApplicationUser> userManager)
        {
            _hostEnvironment = hostEnvironment;
            _guestReviewsRepository = guestReviewsRepository;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<IActionResult> UpdateReview(ReviewModel model)
        {
            var existingReview =  _guestReviewsRepository.Get(await getCurrentUser());
            if (existingReview != null)
            {
                existingReview.Name = model.Name;
                existingReview.Country = model.Country;
                existingReview.Review = model.Review;
                existingReview.Suggestions = model.Suggestions;
                existingReview.Score = model.Score;
                existingReview.GuestPermission = model.GuestPermission;
                existingReview.DateCreated = DateTime.UtcNow;
                existingReview.Approved = false;
            }
            else
            {
                existingReview = new GuestReview(model.Name, model.Country, model.Review, model.Suggestions, model.Score, model.GuestPermission, false, await getCurrentUser());
            }
            _guestReviewsRepository.AddOrUpdate(existingReview);
            String resultUrl = currentLanguageOrDefault() + "/ReviewSavedPage";
            return View(resultUrl);
        }

        public IActionResult GeneralInfo()
        {
            String resultUrl = currentLanguageOrDefault() + "/GeneralInfo";
            return View(resultUrl);
        }

        public IActionResult BeforeArrival()
        {
            String resultUrl = currentLanguageOrDefault() + "/BeforeArrival";
            return View(resultUrl);
        }

        public IActionResult DuringStay()
        {
            String resultUrl = currentLanguageOrDefault() + "/DuringStay";
            return View(resultUrl);
        }

        public IActionResult DayOfDeparture()
        {
            String resultUrl = currentLanguageOrDefault() + "/DayOfDeparture";
            return View(resultUrl);
        }

        public async Task<IActionResult> ReviewPage()
        {
            var review = _guestReviewsRepository.Get(await getCurrentUser());
            ReviewModel model = new ReviewModel();
            if (review != null)
            {
                model.Name = review.Name;
                model.Country = review.Country;
                model.Review = review.Review;
                model.Suggestions = review.Suggestions;
                model.Score = review.Score;
                model.GuestPermission = review.GuestPermission;
            }
            String resultUrl = currentLanguageOrDefault() + "/ReviewPage";
            return View(resultUrl, model);
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

        private async Task<Guid> getCurrentUser()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var currId = new Guid(currentUser.Id);
            return currId;
        }
    }
}