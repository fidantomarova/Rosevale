using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rosevale.DAL;
using Rosevale.Models;
using Rosevale.ViewModels;
using System.Diagnostics;

namespace Rosevale.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db)
        {
            _db = db;
        }
        #region Index
        public async Task<IActionResult> Index()
        {
            HomeVM vm = new HomeVM
            {
                Carousels = await _db.Carousels.Where(x => !x.IsDeactive).ToListAsync(),
                About = await _db.Abouts.Where(x => !x.IsDeactive).FirstOrDefaultAsync(),
                Rooms = await _db.Rooms.OrderByDescending(x => x.Id).Take(3).Where(x => !x.IsDeactive).ToListAsync(),
                Services = await _db.Services.Where(x => !x.IsDeactive).ToListAsync(),
                Testimonials = await _db.Testimonials.Where(x => !x.IsDeactive).ToListAsync(),
                SocialMedias = await _db.SocialMedias.ToListAsync(),
                Staffs = await _db.Staffs.OrderByDescending(x => x.Id).Where(x => !x.IsDeactive).ToListAsync(),

            };
            return View(vm);
        }
        #endregion
    }
}
