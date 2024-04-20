using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.DemoProject.Data;
using MVC.DemoProject.Models;
using System.Diagnostics;

namespace MVC.DemoProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            int userCount = _dbContext.Users.Count();
            int stdCount=_dbContext.Students.Count();
            int imgCount = _dbContext.Images.Count();


            ViewBag.UserCount = userCount;
            ViewBag.StdCount = stdCount;
            ViewBag.ImgCount = imgCount;

            return View();
        }


        [Authorize]
        public IActionResult ChatRoom()
        {
            return View();
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
    }
}
