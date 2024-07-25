using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rosevale.DAL;
using Rosevale.Models;
using Rosevale.ViewModels;

namespace Rosevale.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly AppDbContext _db;
        public ReservationsController(AppDbContext db)
        {
            _db = db;
        }
        #region Index
        public async Task<IActionResult> Index()
        {
            ReservationVM reservationVM = new ReservationVM
            {
                About = await _db.Abouts.FirstOrDefaultAsync(),
                Rooms = await _db.Rooms.ToListAsync(),
                Reservation = await _db.Reservations.FirstOrDefaultAsync(),
            };
           
            return View(reservationVM);
        }
        #endregion

        #region Thanks
        public IActionResult Thanks()
        {
            return View();
        }
        #endregion

        #region Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationVM reservationVM)
        {
            _db.Reservations.Add(reservationVM.Reservation);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Thanks));
        }
        #endregion

    }
}
