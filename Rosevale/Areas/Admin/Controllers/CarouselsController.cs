using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rosevale.DAL;
using Rosevale.Helpers;
using Rosevale.Models;

namespace Rosevale.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Staff")]
    public class CarouselsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CarouselsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        #region Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.Carousels.OrderByDescending(x => x.Id).ToListAsync());
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Carousel carousel)
        {
            if (carousel.Photo == null)
            {
                ModelState.AddModelError("Photo", "Image can not be null");
                return View();
            }
            if (!carousel.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "You must select image");
                return View();
            }
            if (!carousel.Photo.IsOlder1Mb())
            {
                ModelState.AddModelError("Photo", "Max 1 Mb");
                return View();
            }
            string folder = Path.Combine(_env.WebRootPath, "img");
            carousel.Image = await carousel.Photo.SaveFile(folder);
            await _context.AddAsync(carousel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }

            Carousel? carousel = await _context.Carousels.FirstOrDefaultAsync(m => m.Id == id);

            if (carousel == null)
            {
                return RedirectToAction("BadRequest");
            }

            return View(carousel);
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }
            Carousel? carousel = await _context.Carousels.FindAsync(id);
            if (carousel == null)
            {
                return RedirectToAction("BadRequest");
            }

            return View(carousel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Carousel carousel)
        {
            if (id != carousel.Id)
            {
                return RedirectToAction("BadRequest");
            }
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }
            Carousel? dbCarousel = await _context.Carousels.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCarousel == null)
            {
                return RedirectToAction("BadRequest");
            }
            if (carousel.Photo != null)
            {
                if (!carousel.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "You must select image");
                    return View();
                }
                if (!carousel.Photo.IsOlder1Mb())
                {
                    ModelState.AddModelError("Photo", "Max 1 Mb");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "img");
                string path = Path.Combine(folder, dbCarousel.Image);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                dbCarousel.Image = await carousel.Photo.SaveFile(folder);
            }
            dbCarousel.Title = carousel.Title;
            dbCarousel.Desc = dbCarousel.Desc;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }
            Carousel? dbCarousel = await _context.Carousels.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCarousel == null)
            {
                return RedirectToAction("BadRequest");
            }
            if (dbCarousel.IsDeactive)
            {
                dbCarousel.IsDeactive = false;
            }
            else
            {
                dbCarousel.IsDeactive = true;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region ErrorPages
        public IActionResult BadRequest()
        {
            return View();
        }
        public IActionResult NotFound()
        {
            return View();
        }
        #endregion
    }
}
