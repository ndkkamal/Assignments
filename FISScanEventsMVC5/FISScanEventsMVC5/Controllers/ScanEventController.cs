using Microsoft.AspNetCore.Mvc;

namespace FISScanEventsMVC5.Controllers
{
    public class ScanEventController : Controller
    {
        // Get Scan Events
        public IActionResult Index()
        {
            return View();
        }
    }
}
