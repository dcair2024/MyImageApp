using Microsoft.AspNetCore.Mvc;

namespace SeuProjeto.Areas.User.Controllers
{
    [Area("User")]
    public class ImageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Generate(string prompt)
        {
            return Content("Recebi: " + prompt);
        }
    }
}
