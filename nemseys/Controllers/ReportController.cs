using Nemesys.Models;
using Nemesys.Models.Interfaces;
using Nemesys.Models.Repositories;
using Nemesys.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Bloggy.ViewModels;

namespace Nemesys.Controllers
{
    public class ReportController : Controller
    {
        private readonly INemeseysRepository _nemeseysRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportController(INemeseysRepository nemeseysRepository, UserManager<ApplicationUser> userManager)
        {
            _nemeseysRepository = nemeseysRepository;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult Index()
        {
            var reports = _nemeseysRepository.GetAllReports().OrderByDescending(b => b.DateOfReport);
            var model = new ReportListViewModel()
            {
                TotalEntries = _nemeseysRepository.GetAllReports().Count(),
                Reports = _nemeseysRepository
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
                    Upvotes = b.Upvotes,

                    Author = new AuthorViewModel()
                    {
                        Id = b.UserId,
                        Name = (_userManager.FindByIdAsync(b.UserId).Result != null) ?
                            _userManager.FindByIdAsync(b.UserId).Result.UserName : "Anonymous"
                    }
                })
            };
            return View(model);
        }
        public IActionResult Details(int id)
        {
            var report = _nemeseysRepository.GetReportById(id);
            if (report == null)
                return NotFound();
            else
            {
                var model = new ReportViewModel()
                {
                    ReportId = report.ReportId,
                    DateOfReport = report.DateOfReport,
                    HazardLocation = report.HazardLocation,
                    DateAndTimeSpotted = report.DateAndTimeSpotted,
                    TypeOfHazard = report.TypeOfHazard,
                    TitleOfReport = report.TitleOfReport,
                    Description = report.Description,
                    Status = report.Status,
                    ImageUrl = report.ImageUrl,
                    Upvotes = report.Upvotes,

                    Author = new AuthorViewModel()
                    {
                        Id = report.UserId,
                        Name = (_userManager.FindByIdAsync(report.UserId).Result != null) ?
                            _userManager.FindByIdAsync(report.UserId).Result.UserName : "Anonymous"
                    }
                };
                return View(model);
            }
        }

        [Authorize(Roles = "Reporter")]
        [HttpGet]
        public IActionResult Create()
        {

            var model = new EditReportViewModel()
            {
                DateOfReport = DateTime.Now,
                Status = "Open"
            };

            return View(model);
        }

        [Authorize(Roles = "Reporter")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("TitleOfReport, Description, ImageToUpload, DateOfReport, HazardLocation, DateAndTimeSpotted, TypeOfHazard, Status")] EditReportViewModel newReport)
        {
            if (ModelState.IsValid)
            {
                string fileName = "";
                if (newReport.ImageToUpload != null && newReport.ImageToUpload.Length > 0)
                {
                    // Check for file aspects such as size, extension, etc., and store with a unique name (e.g., GUID)
                    var extension = "." + newReport.ImageToUpload.FileName.Split('.')[newReport.ImageToUpload.FileName.Split('.').Length - 1];
                    fileName = Guid.NewGuid().ToString() + extension;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "reports", fileName);
                    using (var bits = new FileStream(path, FileMode.Create))
                    {
                        newReport.ImageToUpload.CopyTo(bits);
                    }
                    newReport.ImageUrl = "/images/reports/" + fileName; // Set the ImageUrl to the new file path
                }
                else
                {
                    newReport.ImageUrl = "/images/reports/default.jpg"; // replace with your actual default image path
                }

                Report report = new Report()
                {
                    DateOfReport = newReport.DateOfReport,
                    HazardLocation = newReport.HazardLocation,
                    DateAndTimeSpotted = newReport.DateAndTimeSpotted,
                    TypeOfHazard = newReport.TypeOfHazard,
                    TitleOfReport = newReport.TitleOfReport,
                    Description = newReport.Description,
                    Status = newReport.Status,
                    ImageUrl = newReport.ImageUrl,
                    Upvotes = 0, // Initialize upvotes, assuming new reports start with zero upvotes
                    UserId = _userManager.GetUserId(User) // Assuming you have user management that associates reports with users
                };

                // Persist to repository
                _nemeseysRepository.CreateReport(report);
                TempData["Message"] = "Report created successfully!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // If the model state is not valid, return to the view with the current model to allow for corrections
                return View(newReport);
            }

        }
        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var existingReport = _nemeseysRepository.GetReportById(id);
            if (existingReport != null)
            {
                // Check if the current user has access to this resource
                var currentUserId = _userManager.GetUserId(User);
                if (existingReport.UserId == currentUserId)
                {
                    EditReportViewModel model = new EditReportViewModel()
                    {
                        ReportId = existingReport.ReportId,
                        DateOfReport = existingReport.DateOfReport,
                        HazardLocation = existingReport.HazardLocation,
                        DateAndTimeSpotted = existingReport.DateAndTimeSpotted,
                        TypeOfHazard = existingReport.TypeOfHazard,
                        TitleOfReport = existingReport.TitleOfReport,
                        Description = existingReport.Description,
                        Status = existingReport.Status,
                        ImageUrl = existingReport.ImageUrl,
                        Upvotes = existingReport.Upvotes,
                    };

                    return View(model);
                }
                else
                    return Forbid(); // Or redirect to an error page or to the Index page
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit([FromRoute] int id, [Bind("ReportId, DateOfReport, HazardLocation, DateAndTimeSpotted, TypeOfHazard, TitleOfReport, Description, Status, ImageToUpload")] EditReportViewModel updatedReport)
        {
            var reportToUpdate = _nemeseysRepository.GetReportById(id);
            if (reportToUpdate == null)
            {
                return NotFound();
            }

            // Check if the current user has access to this resource
            var currentUserId = _userManager.GetUserId(User);
            if (reportToUpdate.UserId == currentUserId)
            {
                if (ModelState.IsValid)
                {
                    if (updatedReport.ImageToUpload != null)
                    {
                        string fileName = "";   
                        var extension = "." + updatedReport.ImageToUpload.FileName.Split('.')[updatedReport.ImageToUpload.FileName.Split('.').Length - 1];
                        fileName = Guid.NewGuid().ToString() + extension;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "reports", fileName);
                        using (var bits = new FileStream(path, FileMode.Create))
                        {
                            updatedReport.ImageToUpload.CopyTo(bits);
                        }
                        reportToUpdate.ImageUrl = "/images/reports/" + fileName;  // Update the imageUrl
                    }

                    // Update other fields
                    reportToUpdate.DateOfReport = updatedReport.DateOfReport;
                    reportToUpdate.HazardLocation = updatedReport.HazardLocation;
                    reportToUpdate.DateAndTimeSpotted = updatedReport.DateAndTimeSpotted;
                    reportToUpdate.TypeOfHazard = updatedReport.TypeOfHazard;
                    reportToUpdate.TitleOfReport = updatedReport.TitleOfReport;
                    reportToUpdate.Description = updatedReport.Description;
                    reportToUpdate.Status = updatedReport.Status;

                    _nemeseysRepository.UpdateReport(reportToUpdate);
                    TempData["Message"] = "Changes saved successfully!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // If the model state is not valid, return to the view with the current model to allow for corrections
                    return View(updatedReport);
                }
            }
            else
                return Forbid(); // Or redirect to an error page or to the Index page
        }

        [Authorize]
        public IActionResult MyReports()
        {
            var userId = _userManager.GetUserId(User);
            var reports = _nemeseysRepository.GetAllReports()
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.DateOfReport);

            var model = new ReportListViewModel()
            {
                TotalEntries = reports.Count(),
                Reports = reports.Select(b => new ReportViewModel
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
                    Upvotes = b.Upvotes,

                    Author = new AuthorViewModel()
                    {
                        Id = b.UserId,
                        Name = (_userManager.FindByIdAsync(b.UserId).Result != null) ?
                            _userManager.FindByIdAsync(b.UserId).Result.UserName : "Anonymous"
                    }
                })
            };

            return View(model);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Upvote(int id)
        {
            var userId = _userManager.GetUserId(User);
            var report = _nemeseysRepository.GetReportById(id);

            if (report != null)
            {
                var cookieKey = $"Upvoted_{id}";
                if (Request.Cookies[cookieKey] == userId)
                {
                    // The user has already voted for this report
                    TempData["Message"] = "You have already voted for this report.";
                    return RedirectToAction("Index", "Home");
                }

                report.Upvotes++;
                _nemeseysRepository.UpdateReport(report);

                Response.Cookies.Append(cookieKey, userId);
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [Authorize]
        public IActionResult Downvote(int id)
        {
            var userId = _userManager.GetUserId(User);
            var report = _nemeseysRepository.GetReportById(id);

            if (report != null)
            {
                var cookieKey = $"Upvoted_{id}";
                if (Request.Cookies[cookieKey] != userId)
                {
                    // The user has already voted for this report
                    TempData["Message"] = "You have not voted for this report yet.";
                    return RedirectToAction("Index", "Home");

                }

                if (report.Upvotes > 0)
                {
                    report.Upvotes--;
                    _nemeseysRepository.UpdateReport(report);

                    Response.Cookies.Delete(cookieKey);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        // This method responds to GET requests and shows the confirmation page
        [HttpGet]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var report = _nemeseysRepository.GetReportById(id);
            if (report == null)
            {
                return NotFound();
            }

            // Check if the current user has access to this resource
            var currentUserId = _userManager.GetUserId(User);
            if (report.UserId == currentUserId)
            {
                return View(report);
            }
            else
            {
                return Forbid(); // Or redirect to an error page or to the Index page
            }
        }

        // This method responds to POST requests and actually deletes the report
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var report = _nemeseysRepository.GetReportById(id);
            if (report == null)
            {
                return NotFound();
            }

            // Check if the current user has access to this resource
            var currentUserId = _userManager.GetUserId(User);
            if (report.UserId == currentUserId)
            {
                _nemeseysRepository.DeleteReport(report);
                TempData["Message"] = "Successfully Deleted The Report!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return Forbid(); // Or redirect to an error page or to the Index page
            }
        }


    }
}
