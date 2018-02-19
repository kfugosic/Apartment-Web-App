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
using ImageResizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ApartmanWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _hostEnvironment;
        private readonly IApplicationSettingsRepository _appSettingsRepository;
        private readonly IGuestReviewsRepository _guestReviewsRepository;
        private readonly IConfiguration _configuration;

        public AdminController(IHostingEnvironment hostEnvironment,
            ILogger<AdminController> logger,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IApplicationSettingsRepository appSettingsRepository,
            IGuestReviewsRepository guestReviewsRepository,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
            _context = context;
            _appSettingsRepository = appSettingsRepository;
            _guestReviewsRepository = guestReviewsRepository;
            _configuration = configuration;
        }

        //
        // REVIEWS
        //

        public IActionResult Reviews()
        {
            ReviewsListModel model = new ReviewsListModel();
            var allReviews = _guestReviewsRepository.getAll();
            foreach (var review in allReviews)
            {
                model.ReviewsList.Add(review);
            }

            return View(model);
        }

        [HttpGet("Approve/{guid}")]
        public IActionResult Approve(string guid)
        {
            _guestReviewsRepository.setApproved(Guid.Parse(guid));
            return RedirectToAction("Reviews");
        }

        [HttpGet("Disapprove/{guid}")]
        public IActionResult Disapprove(string guid)
        {
            _guestReviewsRepository.setDisapproved(Guid.Parse(guid));
            return RedirectToAction("Reviews");
        }

        [HttpGet("DeleteReview/{guid}")]
        public IActionResult DeleteReview(string guid)
        {
            _guestReviewsRepository.Remove(Guid.Parse(guid));
            return RedirectToAction("Reviews");
        }

        //
        // IMAGES
        //

        public IActionResult Images()
        {
            string rootPath = System.IO.Path.Combine(_hostEnvironment.WebRootPath, "images\\apartment");
            HomeViewModel homeViewModel = generateHomeViewModel();
            homeViewModel.DirectReservation = false;
            return View(homeViewModel);
        }

        [HttpGet("MoveUp/{guid}")]
        public IActionResult MoveUp(string guid)
        {
            var appSettings = _appSettingsRepository.Get();
            var imageOrder = appSettings.Order;

            var index1 = -1;
            var index2 = imageOrder.IndexOf($"-{guid}-");
            for (int i = 0; i < index2; i++)
            {
                if (imageOrder.ElementAt(i).Equals('-'))
                {
                    index1 = i;
                }
            }

            if (index1 == -1)
            {
                return RedirectToAction("Images");
            }

            index1++; // Pomakni se sa - na sljedeci broj

            var toReplaceId1 = imageOrder.Substring(index1, index2 - index1);
            Debug.WriteLine($"{imageOrder} {index1} {index2} {toReplaceId1}");

            imageOrder = imageOrder.Replace($"-{guid}-", "-r-");
            imageOrder = imageOrder.Replace($"-{toReplaceId1}-", $"-{guid}-");
            imageOrder = imageOrder.Replace("-r-", $"-{toReplaceId1}-");

            appSettings.Order = imageOrder;
            _appSettingsRepository.Update(appSettings);

            return RedirectToAction("Images");
        }

        [HttpGet("MoveDown/{guid}")]
        public IActionResult MoveDown(string guid)
        {
            var appSettings = _appSettingsRepository.Get();
            var imageOrder = appSettings.Order;

            var index1 = imageOrder.IndexOf($"-{guid}-") + guid.Length + 2;
            var index2 = -1;
            for (int i = index1; i < imageOrder.Length; i++)
            {
                if (imageOrder.ElementAt(i).Equals('-'))
                {
                    index2 = i;
                    break;
                }
            }

            if (index2 == -1)
            {
                return RedirectToAction("Images");
            }

            var toReplaceId1 = imageOrder.Substring(index1, index2 - index1);
            Debug.WriteLine($"{imageOrder} {index1} {index2} {toReplaceId1}");

            imageOrder = imageOrder.Replace($"-{guid}-", "-r-");
            imageOrder = imageOrder.Replace($"-{toReplaceId1}-", $"-{guid}-");
            imageOrder = imageOrder.Replace("-r-", $"-{toReplaceId1}-");

            Debug.WriteLine($"{imageOrder}");

            appSettings.Order = imageOrder;
            _appSettingsRepository.Update(appSettings);

            return RedirectToAction("Images");
        }

        [HttpGet("Delete/{guid}")]
        public IActionResult DeleteImage(string guid)
        {
            string rootPath = System.IO.Path.Combine(_hostEnvironment.WebRootPath, "images\\apartment");
            var path = Path.Combine(rootPath, guid + ".jpg");
            var pathtb = Path.Combine(rootPath, guid + "tb.jpg");
            if (System.IO.File.Exists(path))
            {
                var appSettings = _appSettingsRepository.Get();
                appSettings.Order = appSettings.Order.Replace($"-{guid}-", "-");
                _appSettingsRepository.Update(appSettings);
                System.IO.File.Delete(path);
            }

            if (System.IO.File.Exists(pathtb))
            {
                System.IO.File.Delete(pathtb);
            }

            return RedirectToAction("Images", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> FileUpload(List<IFormFile> files, string position)
        {
            long size = files.Sum(f => f.Length);
            Debug.WriteLine(_hostEnvironment.WebRootPath);
            string rootPath = System.IO.Path.Combine(_hostEnvironment.WebRootPath, "images\\apartment");
            Debug.WriteLine(rootPath);

            string toAddInAppSettingsOrder = "";
            foreach (var formFile in files)
            {
                if (!formFile.FileName.ToLower().EndsWith(".jpg") && !formFile.FileName.ToLower().EndsWith(".jpeg") &&
                    !formFile.FileName.ToLower().EndsWith(".png"))
                {
                    continue;
                }

                var allFiles = Directory.GetFiles(rootPath);
                string filePath = "";
                string thumbPath = "";
                int newFileId = -1;
                for (int i = 0; i < 9999; i++)
                {
                    filePath = System.IO.Path.Combine(rootPath, i + ".jpg");
                    thumbPath = System.IO.Path.Combine(rootPath, i + "tb.jpg");
                    if (!allFiles.Contains(filePath))
                    {
                        newFileId = i;
                        break;
                    }
                }

                if (position.Equals("start"))
                {
                    toAddInAppSettingsOrder += $"-{newFileId}";
                }
                else
                {
                    toAddInAppSettingsOrder += $"{newFileId}-";
                }

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

            if (!toAddInAppSettingsOrder.Equals(""))
            {
                var appSettings = _appSettingsRepository.Get();
                if (position.Equals("start"))
                {
                    appSettings.Order = toAddInAppSettingsOrder + appSettings.Order;
                }
                else
                {
                    appSettings.Order += toAddInAppSettingsOrder;
                }

                _appSettingsRepository.Update(appSettings);
            }

            //return Ok(new { count = files.Count, size, rootPath });
            return RedirectToAction("Images", "Admin");
        }

        //
        // Users
        //

        public IActionResult AddNewUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {UserName = model.Email, Email = model.Email};
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
            if (user.Email.Equals(_configuration["AdminData:AdminEmail"]))
            {
                return RedirectToAction("UserAccounts");
            }

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
            if (user.Email.Equals(_configuration["AdminData:AdminEmail"]))
            {
                return RedirectToAction("UserAccounts");
            }

            await _userManager.DeleteAsync(user);
            _guestReviewsRepository.RemoveForUser(Guid.Parse(guid));
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        private HomeViewModel generateHomeViewModel()
        {
            // string rootPath = System.IO.Path.Combine(_hostEnvironment.WebRootPath, "images\\apartment");
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