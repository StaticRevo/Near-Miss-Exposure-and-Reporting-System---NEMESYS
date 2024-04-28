using Nemesys.Models.Interfaces;
using Nemesys.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nemesys.Models;

namespace Nemesys.Controllers;

public class HomeController : Controller
{
    private readonly INemeseysRepository _nemeseysRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(INemeseysRepository bloggyRepository, UserManager<ApplicationUser> userManager)
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
    public IActionResult Index(string sortOrder, string status)
    {
        // Store the current sort order and status in the ViewBag
        ViewBag.SortOrder = sortOrder;
        ViewBag.Status = status;

        // Retrieve the reports from the database
        var reports = _nemeseysRepository.GetAllReports();

        // Filter the reports based on the status
        if (!string.IsNullOrEmpty(status) && status != "All")
        {
            reports = reports.Where(r => r.Status == status);
        }

        switch (sortOrder)
        {
            case "Upvotes":
                reports = reports.OrderByDescending(r => r.Upvotes);
                break;
            case "DateOfReport":
                reports = reports.OrderByDescending(r => r.DateOfReport);
                break;
            default:
                reports = reports.OrderByDescending(r => r.DateOfReport);
                break;
        }

        // Pass the sorted and filtered reports to the view
        return View(reports.ToList());
    }



    public IActionResult HallOfFame()
    {
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
