using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // <-- Include için gerekli
using System.Security.Claims;
using YemekTarifiWeb.Data;
using YemekTarifiWeb.Models;

namespace YemekTarifiWeb.Controllers
{
    public class RecipeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecipeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? category)
        {
            var recipes = string.IsNullOrEmpty(category)
                ? _context.Recipes.Include(r => r.User).ToList() // <-- Kullanıcıyı dahil et
                : _context.Recipes
                          .Where(r => r.Category == category)
                          .Include(r => r.User) // <-- Kullanıcıyı dahil et
                          .ToList();

            ViewData["SelectedCategory"] = category;
            return View(recipes);
        }

        public IActionResult Details(int id)
        {
            var recipe = _context.Recipes
                                 .Include(r => r.User) // <-- Kullanıcıyı dahil et
                                 .FirstOrDefault(r => r.Id == id);
            if (recipe == null)
                return NotFound();

            return View(recipe);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Recipe recipe, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine("wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    recipe.ImagePath = "/images/" + fileName;
                }

                recipe.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Recipes.Add(recipe);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(recipe);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == id);
            if (recipe == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (recipe.UserId != userId)
                return Forbid();

            return View(recipe);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Recipe updatedRecipe, IFormFile? ImageFile)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == id);
            if (recipe == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (recipe.UserId != userId)
                return Forbid();

            if (ModelState.IsValid)
            {
                recipe.Title = updatedRecipe.Title;
                recipe.Ingredients = updatedRecipe.Ingredients;
                recipe.Instructions = updatedRecipe.Instructions;
                recipe.Category = updatedRecipe.Category;

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    if (!string.IsNullOrEmpty(recipe.ImagePath))
                    {
                        var oldPath = Path.Combine("wwwroot", recipe.ImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine("wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    recipe.ImagePath = "/images/" + fileName;
                }

                _context.SaveChanges();
                return RedirectToAction("Details", new { id = recipe.Id });
            }

            return View(updatedRecipe);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == id);
            if (recipe == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (recipe.UserId != userId)
                return Forbid();

            if (!string.IsNullOrEmpty(recipe.ImagePath))
            {
                var imagePath = Path.Combine("wwwroot", recipe.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }

            _context.Recipes.Remove(recipe);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
