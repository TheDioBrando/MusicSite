using Microsoft.AspNetCore.Mvc;
using RolesSiteMVC.Data;
using RolesSiteMVC.Models;
using System.Diagnostics;

namespace RolesSiteMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var cookie = HttpContext.Request.Cookies;
            var session = HttpContext.Session;
            Music? music = null;

            ViewData["bg-color"] = "aqua";

            if (session.Keys.Contains("color"))
                ViewData["bg-color"] = session.GetString("color");

            if (cookie.ContainsKey("lastMusic"))
            {
                int id;

                if (int.TryParse(cookie["lastMusic"],out id))
                   music = _context.Musics.FirstOrDefault(m => m.Id == id);
            }

            return View(music);
        }

        [HttpPost]
        public IActionResult SaveBgColor(string Color)
        {
            HttpContext.Session.SetString("color", Color);
            return RedirectToAction("Index");
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