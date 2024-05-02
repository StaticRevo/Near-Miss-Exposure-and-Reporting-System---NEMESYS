using Nemesys.Models.Interfaces;
using Nemesys.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nemesys.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

namespace Nemesys.Controllers;

public class HomeController : Controller
{

    private readonly INemeseysRepository _nemeseysRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<HomeController> _logger;

    public HomeController(INemeseysRepository bloggyRepository, UserManager<ApplicationUser> userManager, ILogger<HomeController> logger)
    {
        _nemeseysRepository = bloggyRepository;
        _userManager = userManager;
        _logger = logger;
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
            _logger.LogError(ex.Message);
            return View("Error");
        }

    }
    public IActionResult Index(string sortOrder, string status, string searchTerm)
    {
        try
        {
            // Store the current sort order and status in the ViewBag
            ViewBag.SortOrder = sortOrder;
            ViewBag.Status = status;

            // Retrieve the reports from the database
            var reports = _nemeseysRepository.GetAllReports();

            // Filter the reports based on the search term
            if (!string.IsNullOrEmpty(searchTerm))
            {
                reports = reports.Where(r => r.TitleOfReport.Contains(searchTerm));
            }

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
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return View("Error");
        }
        
    }

    public IActionResult Privacy()
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return View("Error");
        }
    }

    public IActionResult Support()
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return View("Error");
        }
    }

    public IActionResult Error()
    {
        return View();
        //Not handling any errors for Error action
    }

    public async Task<IActionResult> HallOfFame()
    {
        try
        {
            var reporters = await _userManager.Users
            .OrderByDescending(u => u.ClosedReportsCount)
            .ToListAsync();

            return View(reporters);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return View("Error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> SendSupportEmail(string name, string email, string phone, string message)
    {
        //Blank implementation cause I can't put the SMTP info in a public repo

        try
        {
            string senderEmail = ""
            string receiverEmail = "gregory.pavia.22@um.edu.mt"; // I'm sending this to my UOM account due to testing limit on emails. It does actually work - please see documentation for example

            var mailMessage = new MailMessage(senderEmail, receiverEmail);
            mailMessage.Subject = "New Contact Form Submission";
            mailMessage.Body = $"Name: {name}\nEmail: {email}\nPhone: {phone}\nMessage: {message}";

            using (var client = new SmtpClient())
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(); //password
                client.EnableSsl = true;

                await client.SendMailAsync(mailMessage);
            }

            return RedirectToAction("Index", "Home"); // Redirect to home page after sending email
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return View("Error");
        }
    }

    public class ModelBindingTest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public IActionResult About(ModelBindingTest test)
    {
        try
        {
            //Load data from the Model
            ViewBag.Title = "Nemesys - About";
            ViewBag.Message = "Hello there, this is the about page. The model binder found - " + test.Name + " - " + test.Id;
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return View("Error");
        }
    }
}
