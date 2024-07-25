using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rosevale.DAL;
using Rosevale.Models;
using Rosevale.ViewModels;

namespace Rosevale.Controllers
{
    public class RoomsController : Controller
    {
        private readonly AppDbContext _db;
        public RoomsController(AppDbContext db)
        {
            _db = db;
        }
        #region Index
        public async Task<IActionResult> Index(int page = 1)
        {
            int take = 6;
            ViewBag.PagesCount = Math.Ceiling((decimal)await _db.Rooms.CountAsync() / take);
            if (page > ViewBag.PagesCount || page <= 0)
            {
                return RedirectToAction("BadRequest");
            }
            RoomVM roomVM = new RoomVM
            {
                Rooms = await _db.Rooms.OrderByDescending(x => x.Id).Skip((page - 1) * take).Take(take).ToListAsync(),
                Testimonials = await _db.Testimonials.ToListAsync(),
            };
            ViewBag.CurrentPage = page;
            return View(roomVM);
        }
        #endregion

        #region Detail
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }
            Room? room = await _db.Rooms.FirstOrDefaultAsync(x => x.Id == id);
            if (room == null)
            {
                return RedirectToAction("BadRequest");
            }
            return View(room);
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
