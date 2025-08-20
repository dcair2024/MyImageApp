using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyImageApp.Models;
using System.Threading.Tasks;

namespace MyImageApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GerenciarUsuarios()
        {
            var usuarios = _userManager.Users.ToList();
            return View(usuarios);
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirUsuario(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("GerenciarUsuarios");
        }

        [HttpPost]
        public async Task<IActionResult> PromoverAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsAdmin = true;
                await _userManager.UpdateAsync(user);
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            return RedirectToAction("GerenciarUsuarios");
        }
    }
}

