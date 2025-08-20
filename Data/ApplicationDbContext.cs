using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YemekTarifiWeb.Models;
using YemekTarifiWeb.Areas.Identity.Data; // ApplicationUser burada olmalı

namespace YemekTarifiWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> // ApplicationUser eklendi
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }
    }
}
