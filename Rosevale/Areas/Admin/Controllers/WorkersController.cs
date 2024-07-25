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
    public class WorkersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public WorkersController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        #region Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.Staffs.OrderByDescending(x => x.Id).ToListAsync());
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Staff staff)
        {
            if (staff.Photo == null)
            {
                ModelState.AddModelError("Photo", "Image can not be null");
                return View();
            }
            if (!staff.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "You must select image");
                return View();
            }
            if (!staff.Photo.IsOlder1Mb())
            {
                ModelState.AddModelError("Photo", "Max 1 Mb");
                return View();
            }
            string folder = Path.Combine(_env.WebRootPath, "img");
            staff.Image = await staff.Photo.SaveFile(folder);
            await _context.AddAsync(staff);
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

            Staff? staff = await _context.Staffs.FirstOrDefaultAsync(m => m.Id == id);

            if (staff == null)
            {
                return RedirectToAction("BadRequest");
            }

            return View(staff);
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }
            Staff? staff = await _context.Staffs.FindAsync(id);
            if (staff == null)
            {
                return RedirectToAction("BadRequest");
            }

            return View(staff);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Staff staff)
        {
            if (id != staff.Id)
            {
                return RedirectToAction("BadRequest");
            }
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }
            Staff? dbStaff = await _context.Staffs.FirstOrDefaultAsync(x=>x.Id==id);
            if (dbStaff == null)
            {
                return RedirectToAction("BadRequest");
            }
            if (staff.Photo != null)
            {
                if (!staff.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "You must select image");
                    return View();
                }
                if (!staff.Photo.IsOlder1Mb())
                {
                    ModelState.AddModelError("Photo", "Max 1 Mb");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "img");
                string path = Path.Combine(folder, dbStaff.Image);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                dbStaff.Image = await staff.Photo.SaveFile(folder);
            }
            dbStaff.FullName = staff.FullName;
            dbStaff.Designation = staff.Designation;
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
            Staff? dbStaff = await _context.Staffs.FirstOrDefaultAsync(x => x.Id == id);
            if (dbStaff == null)
            {
                return RedirectToAction("BadRequest");
            }
            if (dbStaff.IsDeactive)
            {
                dbStaff.IsDeactive = false;
            }
            else
            {
                dbStaff.IsDeactive = true;
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
