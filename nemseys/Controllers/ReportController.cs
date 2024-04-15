using Nemesis.Models;
using Nemesis.Models.Interfaces;
using Nemesis.Models.Repositories;
using Nemesis.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Nemesis.ViewModels;


namespace Nemesis.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportRepository _reportRepository;

        public ReportController(IReportRepository reportRepository) // Constructor Dependency Injection
        {
            _reportRepository = reportRepository;
        }
        public IActionResult Index() // Listing the Reports
        {
            var reports = _reportRepository.GetAllReports().OrderByDescending(b => b.DateOfReport);
            var model = new ReportListViewModel()
            {
                TotalEntries = _reportRepository.GetAllReports().Count(),
                Reports = _reportRepository
                .GetAllReports()
                .OrderByDescending(b => b.DateOfReport)
                .Select(b => new ReportViewModel
                {
                    ReportId = b.ReportId,
                    DateOfReport = b.DateOfReport,
                    HazardLocation = b.HazardLocation,
                    DateAndTimeSpotted = b.DateAndTimeSpotted,
                    TypeOfHazard = b.TypeOfHazard,
                    TitleOfReport = b.TitleOfReport,
                    Description = b.Description,
                    Status = b.Status,
                    ImageUrl = b.ImageUrl,
                    ReporterEmail = b.Reporter.Email,
                    Upvotes = b.Upvotes,
                    Category = new ReportCategoryViewModel()
                    {
                        Id = b.Category.Id,
                        Name = b.Category.Name
                    }
                })
            };

            return View(model);

        }
        public IActionResult Details(int reportId)
        {
            var reports = _reportRepository.GetReportByID(reportId);
            if (reports == null)
                return NotFound();
            else
            {
                var model = new ReportViewModel()
                {
                    ReportId = reports.ReportId,
                    DateOfReport = reports.DateOfReport,
                    HazardLocation = reports.HazardLocation,
                    DateAndTimeSpotted = reports.DateAndTimeSpotted,
                    TypeOfHazard = reports.TypeOfHazard,
                    TitleOfReport = reports.TitleOfReport,
                    Description = reports.Description,
                    Status = reports.Status,
                    ImageUrl = reports.ImageUrl,
                    ReporterEmail = reports.Reporter.Email,
                    Upvotes = reports.Upvotes,
                    Category = new ReportCategoryViewModel()
                    {
                        Id = reports.Category.Id,
                        Name = reports.Category.Name
                    }
                };
                return View(model);
            }
        }

        [HttpGet]
public IActionResult Create()
{
    // Fetch categories from the repository
    var categories = _reportRepository.GetAllCategories();
    if (categories == null)
    {
        // Handle the case where categories could not be retrieved
        return View("Error"); // Or any other appropriate view/error handling
    }

    // Create a list of ReportCategoryViewModel from the categories
    var categoryList = categories.Select(c => new ReportCategoryViewModel
    {
        Id = c.Id,
        Name = c.Name,
    }).ToList();

    var model = new EditReportViewModel()
    {
        CategoryList = categoryList
    };

    // Pass the model to the view
    return View(model);
}

        [HttpPost]
        public IActionResult Create([Bind("TitleOfReport, Description, ImageToUpload, CategoryId, HazardLocation, DateOfReport, DateAndTimeSpotted, TypeOfHazard, Status, ReporterEmail")] EditReportViewModel newReport)
        {
            if (ModelState.IsValid)
            {
                string fileName = "";
                if (newReport.ImageToUpload != null)
                {
                    var extension = Path.GetExtension(newReport.ImageToUpload.FileName);
                    fileName = Guid.NewGuid().ToString() + extension;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "reports", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        newReport.ImageToUpload.CopyTo(stream);
                    }
                    newReport.ImageUrl = "/images/reports/" + fileName;
                }

                // Create a report instance from the bound model
                Report report = new Report()
                {
                    TitleOfReport = newReport.TitleOfReport,
                    Description = newReport.Description,
                    DateOfReport = newReport.DateOfReport,
                    HazardLocation = newReport.HazardLocation,
                    DateAndTimeSpotted = newReport.DateAndTimeSpotted,
                    TypeOfHazard = newReport.TypeOfHazard,
                    Status = newReport.Status,
                    ImageUrl = "/images/reports" + fileName,
                    Upvotes = 0, // Initialize upvotes as 0 for new reports                                                             §§§
                    
                };

                // Persist the new report into the repository
                _reportRepository.CreateReport(report);
                return RedirectToAction("Index");
            }
            else
            {
                // Load categories again to repopulate the dropdown if there's a validation error
                var categoryList = _reportRepository.GetAllCategories().Select(c => new ReportCategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();

                newReport.CategoryList = categoryList; // Attach categories to the ViewModel

                return View(newReport);
            }
        }
    }
}
