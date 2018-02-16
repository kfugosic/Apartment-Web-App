using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApartmanWeb.Data;
using Microsoft.AspNetCore.Mvc;
using ApartmanWeb.Models;
using ApartmanWeb.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Isam.Esent.Interop;
using Microsoft.Extensions.Logging;

namespace ApartmanWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _hostEnvironment;


        public AdminController(IHostingEnvironment hostEnvironment, ILogger<AdminController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
            _context = context;
        }

        public IActionResult AddNewUser()
        {
            return View();
        }

        public IActionResult Images()
        {
            string rootPath = System.IO.Path.Combine(_hostEnvironment.WebRootPath, "images\\apartment");
            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.ImageCounter = Directory.GetFiles(rootPath).Length / 2;
            homeViewModel.DirectReservation = false;
            return View(homeViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Admin created a new account with password.");
                    return RedirectToAction("UserAccounts");
                }
                AddErrors(result);
            }
            return View("AddNewUser", model);
        }

        public async Task<IActionResult> UserAccounts()
        {
            var allUsers = _context.Users.ToList();
            var tupleList = new List<(ApplicationUser, bool)>();
            foreach (var user in allUsers)
            {
                bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                tupleList.Add((user, isAdmin));
            }
            UsersListModel model = new UsersListModel();
            model.UserIsAdminTupleList = tupleList;
            return View(model);
        }

        [HttpGet("RemoveAdmin/{guid}")]
        public async Task<IActionResult> RemoveAdmin(string guid)
        {
            var user = await _userManager.FindByIdAsync(guid);
            await _userManager.RemoveFromRoleAsync(user, "Admin");
            _logger.LogInformation($"{user.Email} removed from admins.");
            return RedirectToAction("UserAccounts");
        }
        [HttpGet("MakeAdmin/{guid}")]
        public async Task<IActionResult> MakeAdmin(string guid)
        {
            var user = await _userManager.FindByIdAsync(guid);
            await _userManager.AddToRoleAsync(user, "Admin");
            _logger.LogInformation($"{user.Email} is now admin.");
            return RedirectToAction("UserAccounts");
        }

        [HttpGet("DeleteAccount/{guid}")]
        public async Task<IActionResult> DeleteAccount(string guid)
        {
            var user = await _userManager.FindByIdAsync(guid);
            await _userManager.DeleteAsync(user);
            _logger.LogInformation($"{user.Email} - {user.PasswordHash} - account deleted.");
            return RedirectToAction("UserAccounts");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register2(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Admin created a new account with password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
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
