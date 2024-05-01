using Nemesys.Models;
using Nemesys.Models.Interfaces;
using Nemesys.Models.Repositories;
using Nemesys.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Bloggy.ViewModels;
using System.Composition;
using Microsoft.Extensions.Logging;


namespace Nemesys.Controllers
{
    public class InvestigationController : Controller
    {
        private readonly INemeseysRepository _nemeseysRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<InvestigationController> _logger;

        public InvestigationController(INemeseysRepository bloggyRepository, UserManager<ApplicationUser> userManager, ILogger<InvestigationController> logger)
        {
            _nemeseysRepository = bloggyRepository;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var investigations = _nemeseysRepository.GetAllInvestigations().OrderByDescending(b => b.DateOfAction);
                var model = new InvestigationListViewModel()
                {
                    TotalEntries = investigations.Count(),
                    Investigations = investigations.Select(i =>
                    {
                        var report = _nemeseysRepository.GetReportById(i.ReportId);
                        return new InvestigationViewModel
                        {
                            InvestigationId = i.InvestigationId,
                            DateOfAction = i.DateOfAction,
                            Status = i.Status,
                            InvestigationTitle = i.InvestigationTitle,
                            Outcome = i.Outcome,
                            Feedback = i.Feedback,
                            ReportId = i.ReportId,

                            TitleOfReport = report.TitleOfReport,
                            ReportDescription = report.Description,
                            ImageUrl = report.ImageUrl,
                            DateOfReport = report.DateOfReport,
                            HazardLocation = report.HazardLocation,
                            DateAndTimeSpotted = report.DateAndTimeSpotted,
                            TypeOfHazard = report.TypeOfHazard,
                            ReportStatus = report.Status,

                            Author = new AuthorViewModel()
                            {
                                Id = i.UserId,
                                Name = (_userManager.FindByIdAsync(i.UserId).Result != null) ?
                                    _userManager.FindByIdAsync(i.UserId).Result.UserName : "Anonymous"
                            }
                        };
                    })
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return View("Error");
            }
            
        }


        public IActionResult Details(int id)
        {
            try
            {
                var investigation = _nemeseysRepository.GetInvestigationById(id);
                if (investigation == null)
                    return NotFound();
                else
                {
                    var report = _nemeseysRepository.GetReportById(investigation.ReportId);
                    var model = new EditInvestigationViewModel()
                    {
                        InvestigationId = investigation.InvestigationId,
                        DateOfAction = investigation.DateOfAction,
                        Status = investigation.Status,
                        InvestigationTitle = investigation.InvestigationTitle,
                        Outcome = investigation.Outcome,
                        Feedback = investigation.Feedback,
                        ReportId = investigation.ReportId,

                        TitleOfReport = report.TitleOfReport,
                        ReportDescription = report.Description,
                        ImageUrl = report.ImageUrl,
                        DateOfReport = report.DateOfReport,
                        HazardLocation = report.HazardLocation,
                        DateAndTimeSpotted = report.DateAndTimeSpotted,
                        TypeOfHazard = report.TypeOfHazard,
                        ReportStatus = report.Status,

                    };
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return View("Error");
            } 
        }

        [Authorize(Roles = "Investigator")]
        [HttpGet]
        public IActionResult Create(int id)
        {
            try
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
                    ReportDescription = report.Description,
                    ImageUrl = report.ImageUrl,
                    DateOfReport = report.DateOfReport,
                    HazardLocation = report.HazardLocation,
                    DateAndTimeSpotted = report.DateAndTimeSpotted,
                    TypeOfHazard = report.TypeOfHazard,
                    ReportStatus = report.Status
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return View("Error");
            }   
        }


        [Authorize(Roles = "Investigator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EditInvestigationViewModel newInvestigation)
        {
            try
            {
                _logger.LogInformation("Create POST method called");
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Model state is valid");
                    Investigation investigation = new Investigation()
                    {
                        ReportId = newInvestigation.ReportId,
                        InvestigationTitle = newInvestigation.InvestigationTitle,
                        DateOfAction = newInvestigation.DateOfAction,
                        Status = newInvestigation.Status,
                        Outcome = newInvestigation.Outcome,
                        Feedback = newInvestigation.Feedback,
                        UserId = _userManager.GetUserId(User)
                    };
                    // Retrieve the report from the database
                    var report = _nemeseysRepository.GetReportById(newInvestigation.ReportId);

                    // Check if the report is null
                    if (report == null)
                    {
                        // If the report is null, return a NotFound result
                        return NotFound();
                    }

                    // Update the status of the report based on the status of the investigation
                    switch (investigation.Outcome)
                    {
                        case "Resolved":
                            report.Status = "Resolved";
                            // Retrieve the user who closed the report
                            var user = await _userManager.GetUserAsync(User);

                            // Increment the user's closed reports count
                            user.ClosedReportsCount++;

                            // Save the changes to the user
                            await _userManager.UpdateAsync(user);
                            break;
                        case "Unresolved":
                            report.Status = "Under Review";
                            break;
                        case "Escalated":
                            report.Status = "Under Review";
                            break;
                        // Add more cases as needed
                        default:
                            report.Status = "Unknown";
                            break;
                    }

                    // Persist the changes to the report
                    _nemeseysRepository.UpdateReport(report);

                    // Persist to repository
                    _nemeseysRepository.CreateInvestigation(investigation);
                    _logger.LogInformation("Investigation created successfully");
                    // Set a success message in TempData
                    TempData["Message"] = "Investigation created successfully!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _logger.LogWarning("Model state is not valid");
                    // If the model state is not valid, retrieve the report data again

                    var report = _nemeseysRepository.GetReportById(newInvestigation.ReportId);
                    if (report != null)
                    {
                        newInvestigation.TitleOfReport = report.TitleOfReport;
                        newInvestigation.ImageUrl = report.ImageUrl;
                        newInvestigation.DateOfReport = report.DateOfReport;
                        newInvestigation.HazardLocation = report.HazardLocation;
                        newInvestigation.DateAndTimeSpotted = report.DateAndTimeSpotted;
                        newInvestigation.TypeOfHazard = report.TypeOfHazard;
                        newInvestigation.ReportStatus = report.Status;
                    }

                    // Pass the updated model back to the view
                    return View(newInvestigation);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return View("Error");
            }
            
        }
        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var existingInvestigation = _nemeseysRepository.GetInvestigationById(id);
                if (existingInvestigation != null)
                {
                    // Check if the current user has access to this resource
                    var currentUserId = _userManager.GetUserId(User);
                    if (existingInvestigation.UserId == currentUserId)
                    {
                        var report = _nemeseysRepository.GetReportById(existingInvestigation.ReportId);
                        var model = new EditInvestigationViewModel()
                        {
                            InvestigationId = existingInvestigation.InvestigationId,
                            DateOfAction = existingInvestigation.DateOfAction,
                            Status = existingInvestigation.Status,
                            InvestigationTitle = existingInvestigation.InvestigationTitle,
                            Outcome = existingInvestigation.Outcome,
                            Feedback = existingInvestigation.Feedback,

                            TitleOfReport = report.TitleOfReport,
                            ReportDescription = report.Description,
                            ImageUrl = report.ImageUrl,
                            DateOfReport = report.DateOfReport,
                            HazardLocation = report.HazardLocation,
                            DateAndTimeSpotted = report.DateAndTimeSpotted,
                            TypeOfHazard = report.TypeOfHazard,
                            ReportStatus = report.Status,

                        };

                        return View(model);
                    }
                    else
                        return Forbid();
                }
                else
                    return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return View("Error");
            }
            
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, EditInvestigationViewModel updatedInvestigation)
        {
            try
            {
                var investigationToUpdate = _nemeseysRepository.GetInvestigationById(id);
                if (investigationToUpdate == null)
                {
                    return NotFound();
                }

                // Check if the current user has access to this resource
                var currentUserId = _userManager.GetUserId(User);
                if (investigationToUpdate.UserId == currentUserId)
                {
                    if (ModelState.IsValid)
                    {
                        // Update investigation properties
                        investigationToUpdate.DateOfAction = updatedInvestigation.DateOfAction;
                        investigationToUpdate.Outcome = updatedInvestigation.Outcome;
                        investigationToUpdate.Feedback = updatedInvestigation.Feedback;
                        investigationToUpdate.Status = updatedInvestigation.Status;
                        investigationToUpdate.InvestigationTitle = updatedInvestigation.InvestigationTitle;

                        // Update the status of the associated report based on the outcome
                        var report = _nemeseysRepository.GetReportById(investigationToUpdate.ReportId);
                        if (report != null)
                        {
                            switch (updatedInvestigation.Outcome)
                            {
                                case "Resolved":
                                    // Retrieve the user who closed the report
                                    var user = await _userManager.GetUserAsync(User);

                                    // Increment the user's closed reports count
                                    user.ClosedReportsCount++;

                                    // Save the changes to the user
                                    await _userManager.UpdateAsync(user);
                                    break;
                                case "Unresolved":
                                case "Escalated":
                                    report.Status = "Under Review";
                                    break;
                                // Add more cases as needed
                                default:
                                    report.Status = "Unknown";
                                    break;
                            }
                            _nemeseysRepository.UpdateReport(report);
                        }
                        _nemeseysRepository.UpdateInvestigation(investigationToUpdate);
                        // Redirect to action and pass a flag indicating success
                        TempData["Message"] = "Changes saved successfully!";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // If the model state is not valid, return to the view with the current model to allow for corrections
                        return View(updatedInvestigation);
                    }
                }
                else
                    return Forbid(); // Or redirect to an error page or to the Index page
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return View("Error");
            }
            
        }

        [Authorize(Roles = "Investigator")]
        public IActionResult MyInvestigations(string status)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var investigations = _nemeseysRepository.GetAllInvestigations()
                    .Where(i => i.UserId == userId);

                var model = new InvestigationListViewModel()
                {
                    TotalEntries = investigations.Count(),
                    Investigations = investigations.Select(i =>
                    {
                        var report = _nemeseysRepository.GetReportById(i.ReportId);
                        return new InvestigationViewModel
                        {
                            InvestigationId = i.InvestigationId,
                            DateOfAction = i.DateOfAction,
                            Status = i.Status,
                            InvestigationTitle = i.InvestigationTitle,
                            Outcome = i.Outcome,
                            Feedback = i.Feedback,
                            ReportId = i.ReportId,

                            TitleOfReport = report.TitleOfReport,
                            ReportDescription = report.Description,
                            ImageUrl = report.ImageUrl,
                            DateOfReport = report.DateOfReport,
                            HazardLocation = report.HazardLocation,
                            DateAndTimeSpotted = report.DateAndTimeSpotted,
                            TypeOfHazard = report.TypeOfHazard,
                            ReportStatus = report.Status,

                            Author = new AuthorViewModel()
                            {
                                Id = i.UserId,
                                Name = (_userManager.FindByIdAsync(i.UserId).Result != null) ?
                                    _userManager.FindByIdAsync(i.UserId).Result.UserName : "Anonymous"
                            }
                        };
                    })
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return View("Error");
            }   
        }

        // This method responds to GET requests and shows the confirmation page
        [HttpGet]
        [Authorize]
        public IActionResult Delete(int id)
        {
            try
            {
                var investigation = _nemeseysRepository.GetInvestigationById(id);
                if (investigation == null)
                {
                    return NotFound();
                }

                // Check if the current user has access to this resource
                var currentUserId = _userManager.GetUserId(User);
                if (investigation.UserId == currentUserId)
                {
                    return View(investigation);
                }
                else
                {
                    return Forbid(); // Or redirect to an error page or to the Index page
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return View("Error");
            }
            
        }

        // This method responds to POST requests and actually deletes the investigation
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var investigation = _nemeseysRepository.GetInvestigationById(id);
                if (investigation == null)
                {
                    return NotFound();
                }

                // Check if the current user has access to this resource
                var currentUserId = _userManager.GetUserId(User);
                if (investigation.UserId == currentUserId)
                {
                    _nemeseysRepository.DeleteInvestigation(investigation);
                    TempData["Message"] = "Successfully Deleted The Investigation!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Forbid(); // Or redirect to an error page or to the Index page
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return View("Error");
            }      
        }
    }
}
