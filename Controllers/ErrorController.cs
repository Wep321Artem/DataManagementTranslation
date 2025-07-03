using Microsoft.AspNetCore.Mvc;

namespace DataManagementTranslation.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }
    }
}
