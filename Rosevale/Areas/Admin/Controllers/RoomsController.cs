using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Rosevale.DAL;
using Rosevale.Helpers;
using Rosevale.Models;

namespace Rosevale.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Staff")]
    public class RoomsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public RoomsController(AppDbContext context,
                               IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        #region Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rooms.OrderByDescending(x => x.Id).ToListAsync());
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }

            Room? room = await _context.Rooms.FirstOrDefaultAsync(m => m.Id == id);

            if (room == null)
            {
                return RedirectToAction("BadRequest");
            }

            return View(room);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Room room, int rate, int bedCount, int bathCount)
        {
            if (room.Photo == null)
            {
                ModelState.AddModelError("Photo", "Image can not be null");
                return View();
            }
            if (!room.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "You must select image");
                return View();
            }
            if (!room.Photo.IsOlder1Mb())
            {
                ModelState.AddModelError("Photo", "Max 1 Mb");
                return View();
            }
            string folder = Path.Combine(_env.WebRootPath, "img");
            room.Image = await room.Photo.SaveFile(folder);

            await _context.AddAsync(room);
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
            Room? room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return RedirectToAction("BadRequest");
            }

            return View(room);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Room room, int rate, int bedCount, int bathCount)
        {
            if (id != room.Id)
            {
                return RedirectToAction("BadRequest");
            }
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }
            Room? dbRoom = await _context.Rooms.FirstOrDefaultAsync(x => x.Id == id);
            if (dbRoom == null)
            {
                return RedirectToAction("BadRequest");
            }
            #region isExist
            bool isExist = await _context.Rooms.AnyAsync(x => x.Name == room.Name && x.Id != id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This room is already exist");
                return View();
            }
            #endregion
            if (room.Photo != null)
            {
                if (!room.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "You must select image");
                    return View();
                }
                if (!room.Photo.IsOlder1Mb())
                {
                    ModelState.AddModelError("Photo", "Max 1 Mb");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "img");
                string path = Path.Combine(folder, dbRoom.Image);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                dbRoom.Image = await room.Photo.SaveFile(folder);
            }
            dbRoom.Name = room.Name;
            dbRoom.Desc = room.Desc;
            dbRoom.Price = room.Price;
            dbRoom.Rate = room.Rate;
            dbRoom.BedCount = room.BedCount;
            dbRoom.BathCount = room.BathCount;
            dbRoom.IsWifi = room.IsWifi;
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
            Room? dbRoom = await _context.Rooms.FirstOrDefaultAsync(x => x.Id == id);
            if (dbRoom == null)
            {
                return RedirectToAction("BadRequest");
            }
            if (dbRoom.IsDeactive)
            {
                dbRoom.IsDeactive = false;
            }
            else
            {
                dbRoom.IsDeactive = true;
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
