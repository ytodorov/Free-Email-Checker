using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Free_Email_Checker.Models;
using System.Threading;
using Microsoft.Extensions.Configuration;
using NeverBounce.Models;
using NeverBounce;

namespace Free_Email_Checker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IConfiguration configuration;

        public HomeController(IServiceProvider serviceProvider)
        {
            logger = serviceProvider.GetService<ILogger<HomeController>>();
            configuration = serviceProvider.GetService<IConfiguration>();
        }

        public IActionResult Index()
        {
            BaseViewModel baseViewModel = new BaseViewModel();
            baseViewModel.PageBaseCanonicalUrl = configuration["PageBaseCanonicalUrl"];
            return View(baseViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> CheckEmail(string email)
        {
            

            var neverBounceKey = configuration["NeverBounceKey"];

            var sdk = new NeverBounceSdk(neverBounceKey);

            // Create request model
            var model = new SingleRequestModel();
            model.email = email;
            model.credits_info = true;
            model.address_info = true;
            model.timeout = 10;

            // Verify single email
            SingleResponseModel resp = await sdk.Single.Check(model);

            
            return Json(resp);

        }
    }
}
