using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nemesys.Models.Interfaces;
using Nemesys.Models;

namespace Nemesys.Controllers
{
    public class ErrorController : Controller
    {
        //Error controller handles error codes such as 404
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("Error/{statusCode}")]
        public IActionResult Error(int? statusCode)
        {
            try //Error handling incase we dont have a page for a certain error code
            {
                if (statusCode.HasValue)
                {
                    if (statusCode == 404)
                    {
                        return View("404");
                    }
                    else
                    {
                        return View("404");
                    }

                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return View("Error");
            }
        }
    }
}
