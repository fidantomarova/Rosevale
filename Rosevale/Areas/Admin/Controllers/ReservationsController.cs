using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rosevale.DAL;
using Rosevale.Models;
using Rosevale.ViewModels;

namespace Rosevale.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Staff")]
    public class ReservationsController : Controller
    {
        private readonly AppDbContext _context;
        public ReservationsController(AppDbContext context)
        {
            _context = context;
        }
        #region Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reservations.OrderByDescending(x => x.Id).Include(r => r.Room).ToListAsync());
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            TempData["DateFormat"] = "dd-MM-yyyy HH:mm";
            var rooms = _context.Rooms.ToList();

            ViewBag.Rooms = rooms.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            }).ToList();

            var model = new ReservationVM
            {
                Reservation = new Reservation(),
                Rooms = rooms
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationVM reservationVM, int adultCount, int childCount)
        {
            adultCount = reservationVM.Reservation.Adults;
            childCount = reservationVM.Reservation.Children;
            await _context.AddAsync(reservationVM.Reservation);
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

            var reservation = await _context.Reservations
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reservation == null)
            {
                return RedirectToAction("BadRequest");
            }

            return View(reservation);
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }

            var reservation = await _context.Reservations
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reservation == null)
            {
                return RedirectToAction("BadRequest");
            }

            TempData["DateFormat"] = "dd-MM-yyyy HH:mm";

            var rooms = _context.Rooms.ToList();
            ViewBag.Rooms = rooms.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            }).ToList();

            var model = new ReservationVM
            {
                Reservation = reservation,
                Rooms = rooms
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, ReservationVM reservationVM, int adultCount, int childCount)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }

            var dbReservation = await _context.Reservations
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (dbReservation == null)
            {
                return RedirectToAction("BadRequest");
            }
            dbReservation.Name = reservationVM.Reservation.Name;
            dbReservation.Email = reservationVM.Reservation.Email;
            dbReservation.RoomId = reservationVM.Reservation.RoomId;
            dbReservation.CheckIn = reservationVM.Reservation.CheckIn;
            dbReservation.CheckOut = reservationVM.Reservation.CheckOut;
            dbReservation.Adults = reservationVM.Reservation.Adults;
            dbReservation.Children = reservationVM.Reservation.Children;
            dbReservation.SpecialRequest = reservationVM.Reservation.SpecialRequest;
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

            var reservation = await _context.Reservations
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reservation == null)
            {
                return RedirectToAction("NotFound");
            }
            if (reservation.IsDeactive)
            {
                reservation.IsDeactive = false;
            }
            else
            {
                reservation.IsDeactive = true;
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
