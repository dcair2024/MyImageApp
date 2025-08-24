using Microsoft.AspNetCore.Mvc;
using System.Web; // Adicione para usar o HttpUtility

namespace MyImageApp.Areas.User.Controllers
{
    [Area("User")]
    public class ImageController : Controller
    {
        // GET: /User/Image/
        public IActionResult Index()
        {
            return View();
        }

        // POST: /User/Image/Generate
        [HttpPost]
        public IActionResult Generate(string prompt)
        {
            // Simula URL de imagem (fake)
            string fakeImageUrl = "https://via.placeholder.com/512x512.png?text=" + HttpUtility.UrlEncode(prompt);

            // Retorna a URL da imagem como um objeto JSON
            return Json(new { imageUrl = fakeImageUrl });
        }
    }
}


