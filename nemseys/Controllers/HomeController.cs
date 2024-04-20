using Nemesys.Models.Interfaces;
using Nemesys.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Nemesys.Controllers;

public class HomeController : Controller
{
    private readonly INemeseysRepository _nemeseysRepository;
    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(INemeseysRepository bloggyRepository, UserManager<IdentityUser> userManager)
    {
        _nemeseysRepository = bloggyRepository;
        _userManager = userManager;
    }

    [HttpGet]
    [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)]

    [Authorize(Roles = "Admin")]
    public IActionResult Dashboard()
    {
        try
        {
            var model = new ReportDashboardViewModel();
            model.TotalRegisteredUsers = _userManager.Users.Count();
            model.TotalEntries = _nemeseysRepository.GetAllReports().Count();

            return View(model);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex.Message); //More on this soon
            return View("Error");
        }

    }
    public IActionResult Index()
    {
        //Load data from the Model
        ViewBag.Title = "Nemesys - Home";
        ViewBag.Message = "Hello!";
        return View();
    }

    public class ModelBindingTest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public IActionResult About(ModelBindingTest test)
    {
        //Load data from the Model
        ViewBag.Title = "Nemesys - About";
        ViewBag.Message = "Hello there, this is the about page. The model binder found - " + test.Name + " - " + test.Id;
        return View();
    }
}
