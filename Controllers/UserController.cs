using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyImageApp.Data;
using MyImageApp.Models;
using MyImageApp.Services;

[Authorize(Roles = "User")]
public class UserController : Controller
{
    private readonly ImageGenerationService _imageService;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(ImageGenerationService imageService, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _imageService = imageService;
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }


    public IActionResult Dashboard()
    {
        var userId = _userManager.GetUserId(User);
        var images = _context.Images.Where(i => i.UserId == userId).ToList();
        return View(images);
    }

    [HttpPost]
    public async Task<IActionResult> GenerateImage(string prompt)
    {
        var url = await _imageService.GenerateImageAsync(prompt);
        var image = new ImageModel { UserId = _userManager.GetUserId(User), Url = url, CreatedAt = DateTime.Now };
        _context.Images.Add(image);
        await _context.SaveChangesAsync();
        return RedirectToAction("Dashboard");
    }
}
