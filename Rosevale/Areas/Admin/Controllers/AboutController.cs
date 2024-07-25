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
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public AboutController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        #region Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.Abouts.ToListAsync());
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(About about)
        {
            if (about.Photo == null)
            {
                ModelState.AddModelError("Photo", "Image can not be null");
                return View();
            }
            if (!about.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "You must select image");
                return View();
            }
            if (!about.Photo.IsOlder1Mb())
            {
                ModelState.AddModelError("Photo", "Max 1 Mb");
                return View();
            }
            string folder = Path.Combine(_env.WebRootPath, "img");
            about.Img1 = await about.Photo.SaveFile(folder);
            about.Img2 = await about.Photo.SaveFile(folder);
            about.Img3 = await about.Photo.SaveFile(folder);
            about.Img4 = await about.Photo.SaveFile(folder);
            await _context.AddAsync(about);
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

            About? about = await _context.Abouts.FirstOrDefaultAsync(m => m.Id == id);

            if (about == null)
            {
                return RedirectToAction("BadRequest");
            }

            return View(about);
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }
           About? about = await _context.Abouts.FindAsync(id);
            if (about == null)
            {
                return RedirectToAction("BadRequest");
            }

            return View(about);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,About about)
        {
            if (id !=about.Id)
            {
                return RedirectToAction("BadRequest");
            }
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }
           About? dbAbout = await _context.Abouts.FirstOrDefaultAsync(x => x.Id == id);
            if (dbAbout == null)
            {
                return RedirectToAction("BadRequest");
            }
            if (about.Photo != null)
            {
                if (!about.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "You must select image");
                    return View();
                }
                if (!about.Photo.IsOlder1Mb())
                {
                    ModelState.AddModelError("Photo", "Max 1 Mb");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "img");
                string path1 = Path.Combine(folder, dbAbout.Img1);
                string path2 = Path.Combine(folder, dbAbout.Img2);
                string path3 = Path.Combine(folder, dbAbout.Img3);
                string path4 = Path.Combine(folder, dbAbout.Img4);
                if (System.IO.File.Exists(path1))
                {
                    System.IO.File.Delete(path1);
                }
                if (System.IO.File.Exists(path2))
                {
                    System.IO.File.Delete(path2);
                }
                if (System.IO.File.Exists(path3))
                {
                    System.IO.File.Delete(path3);
                }
                if (System.IO.File.Exists(path4))
                {
                    System.IO.File.Delete(path4);
                }

                dbAbout.Img1 = await about.Photo.SaveFile(folder);
                dbAbout.Img2 = await about.Photo.SaveFile(folder);
                dbAbout.Img3 = await about.Photo.SaveFile(folder);
                dbAbout.Img4 = await about.Photo.SaveFile(folder);
            }
            dbAbout.Title =about.Title;
            dbAbout.Name =about.Name;
            dbAbout.Desc =about.Desc;
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
            About? dbAbout = await _context.Abouts.FirstOrDefaultAsync(x => x.Id == id);
            if (dbAbout == null)
            {
                return RedirectToAction("BadRequest");
            }
            if (dbAbout.IsDeactive)
            {
                dbAbout.IsDeactive = false;
            }
            else
            {
                dbAbout.IsDeactive = true;
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
