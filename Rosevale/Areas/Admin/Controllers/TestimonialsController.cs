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
    public class TestimonialsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public TestimonialsController(AppDbContext context,
                                      IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        #region Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.Testimonials.OrderByDescending(x => x.Id).ToListAsync());
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }

            Testimonial? testimonial = await _context.Testimonials.FirstOrDefaultAsync(m => m.Id == id);

            if (testimonial == null)
            {
                return RedirectToAction("BadRequest");
            }

            return View(testimonial);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Testimonial testimonial)
        {
            if (testimonial.Photo == null)
            {
                ModelState.AddModelError("Photo", "Image can not be null");
                return View();
            }
            if (!testimonial.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "You must select image");
                return View();
            }
            if (!testimonial.Photo.IsOlder1Mb())
            {
                ModelState.AddModelError("Photo", "Max 1 Mb");
                return View();
            }
            string folder = Path.Combine(_env.WebRootPath, "img");
            testimonial.Image = await testimonial.Photo.SaveFile(folder);

            await _context.AddAsync(testimonial);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }
            Testimonial? testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial == null)
            {
                return RedirectToAction("BadRequest");
            }

            return View(testimonial);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Testimonial testimonial)
        {
            if (id != testimonial.Id)
            {
                return RedirectToAction("BadRequest");
            }
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }
            Testimonial? dbTestimonial = await _context.Testimonials.FirstOrDefaultAsync(x => x.Id == id);
            if (dbTestimonial == null)
            {
                return RedirectToAction("BadRequest");
            }

            if (testimonial.Photo != null)
            {
                if (!testimonial.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "You must select image");
                    return View();
                }
                if (!testimonial.Photo.IsOlder1Mb())
                {
                    ModelState.AddModelError("Photo", "Max 1 Mb");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "img");
                string path = Path.Combine(folder, dbTestimonial.Image);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                dbTestimonial.Image = await testimonial.Photo.SaveFile(folder);
            }
            dbTestimonial.ClientName = testimonial.ClientName;
            dbTestimonial.Profession = testimonial.Profession;
            dbTestimonial.Desc = testimonial.Desc;
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
            Testimonial? dbTestimonial = await _context.Testimonials.FirstOrDefaultAsync(x => x.Id == id);
            if (dbTestimonial == null)
            {
                return RedirectToAction("BadRequest");
            }
            if (dbTestimonial.IsDeactive)
            {
                dbTestimonial.IsDeactive = false;
            }
            else
            {
                dbTestimonial.IsDeactive = true;
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
