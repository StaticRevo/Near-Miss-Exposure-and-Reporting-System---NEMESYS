using Nemesys.Models;
using Nemesys.Models.Interfaces;
using Nemesys.Models.Repositories;
using Nemesys.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Bloggy.ViewModels;
using System.Composition;


namespace Nemesys.Controllers
{
    public class InvestigationController : Controller
    {
        private readonly INemeseysRepository _nemeseysRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public InvestigationController(INemeseysRepository bloggyRepository, UserManager<ApplicationUser> userManager)
        {
            _nemeseysRepository = bloggyRepository;
            _userManager = userManager;
        }

        public IActionResult Index(string sortOrder)
        {
            // Retrieve the investigations from the database
            var investigations = _nemeseysRepository.GetAllInvestigations();

            switch (sortOrder)
            {
                case "DateOfAction":
                    investigations = investigations.OrderByDescending(i => i.DateOfAction);
                    break;
                case "Status":
                    investigations = investigations.OrderByDescending(i => i.Status);
                    break;
                default:
                    investigations = investigations.OrderByDescending(i => i.DateOfAction);
                    break;
            }

            // Pass the sorted investigations to the view
            return View(investigations);
        }
        [Authorize(Roles = "Investigator")]
        [HttpGet]
        public IActionResult Create(int id)
        {
            // Retrieve the report from the database
            var report = _nemeseysRepository.GetReportById(id);

            // Check if the report is null
            if (report == null)
            {
                // If the report is null, return a NotFound result
                return NotFound();
            }

            // Create a new instance of EditInvestigationViewModel and populate it with the report data
            var model = new EditInvestigationViewModel
            {
                ReportId = id,
                DateOfAction = DateTime.Now,
                Status = "Open",
                TitleOfReport = report.TitleOfReport,
                ImageUrl = report.ImageUrl,
                DateOfReport = report.DateOfReport,
                HazardLocation = report.HazardLocation,
                DateAndTimeSpotted = report.DateAndTimeSpotted,
                TypeOfHazard = report.TypeOfHazard,
                ReportStatus = report.Status
            };

            return View(model);
        }


        [Authorize(Roles = "Investigator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ReportId, InvestigationTitle, DateOfAction, Status, Feedback")] Investigation newInvestigation)
        {
           
            if (ModelState.IsValid)
            {
                Investigation investigation = new Investigation()
                {
                    ReportId = newInvestigation.ReportId,
                    InvestigationTitle = newInvestigation.InvestigationTitle,
                    DateOfAction = newInvestigation.DateOfAction,
                    Status = newInvestigation.Status,
                    Feedback = newInvestigation.Feedback,
                    UserId = _userManager.GetUserId(User)
                };
                // Persist to repository
                _nemeseysRepository.CreateInvestigation(newInvestigation);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // If the model state is not valid, return to the view with the current model to allow for corrections
                return View(newInvestigation);
            }
        }


    }
}
