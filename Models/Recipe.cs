using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YemekTarifiWeb.Areas.Identity.Data; // ApplicationUser burada tanımlı

namespace YemekTarifiWeb.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Malzemeler alanı zorunludur.")]
        public string Ingredients { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tarif alanı zorunludur.")]
        public string Instructions { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kategori alanı zorunludur.")]
        public string Category { get; set; } = string.Empty;

        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}
