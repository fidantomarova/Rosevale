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
    public class ServicesController : Controller
    {
        private readonly AppDbContext _context;
        public ServicesController(AppDbContext context)
        {
            _context = context;
        }
        #region Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.Services.OrderByDescending(x => x.Id).ToListAsync());
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }

            Service? service = await _context.Services.FirstOrDefaultAsync(m => m.Id == id);

            if (service == null)
            {
                return RedirectToAction("BadRequest");
            }

            return View(service);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service service)
        {
            await _context.AddAsync(service);
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
            Service? service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return RedirectToAction("BadRequest");
            }

            return View(service);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Service service)
        {
            if (id != service.Id)
            {
                return RedirectToAction("BadRequest");
            }
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }
            Service? dbService = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (dbService == null)
            {
                return RedirectToAction("BadRequest");
            }
            #region isExist
            bool isExist = await _context.Services.AnyAsync(x => x.Title == service.Title && x.Id != id);
            if (isExist)
            {
                ModelState.AddModelError("Title", "This service is already exist");
                return View();
            }
            #endregion

            dbService.Title = service.Title;
            dbService.Desc = service.Desc;
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
            Service? dbService = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (dbService == null)
            {
                return RedirectToAction("BadRequest");
            }
            if (dbService.IsDeactive)
            {
                dbService.IsDeactive = false;
            }
            else
            {
                dbService.IsDeactive = true;
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
