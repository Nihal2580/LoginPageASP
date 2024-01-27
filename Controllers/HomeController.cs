using LoginPageASP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace LoginPageASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly LoginDbContext context;

        public HomeController(LoginDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return RedirectToAction("DashBoard");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserTab user)
        {
            var myUser = context.UserTabs.Where(x =>x.Email == user.Email && x.Password==user.Password).FirstOrDefault();
            if (myUser != null)
            {
                HttpContext.Session.SetString("UserSession", myUser.Email);
                return RedirectToAction("DashBoard");

            }
            else
            {
                ViewBag.Message = "Login Fail";
            }
            return View();
        }

        public IActionResult DashBoard()
        {
           if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("Login");
            }

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserTab user)
        {
            if(ModelState.IsValid)
            {
                await context.UserTabs.AddAsync(user);
                await context.SaveChangesAsync();
                TempData["Success"] = "Registration Successfully";
                return RedirectToAction("Login");
            }
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
