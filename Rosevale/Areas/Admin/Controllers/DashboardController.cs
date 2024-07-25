using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rosevale.DAL;
using Rosevale.Models;
using Rosevale.ViewModels;

namespace Rosevale.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Staff")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        #region Index
        public IActionResult Index()
        {
            var reservationsCount = _context.Reservations.Count(x => !x.IsDeactive);
            var usersCount = _context.Users.Count();
            var roomsCount = _context.Rooms.Count(x => !x.IsDeactive);
            var unrepliedMessages = _context.Messages.Count(x => !x.IsReplied);

            DashboardVM vm = new DashboardVM
            {
                ReservationsCount = reservationsCount,
                UsersCount = usersCount,
                RoomsCount = roomsCount,
                UnrepliedMessages = unrepliedMessages
            };
            return View(vm);
        }
        #endregion
    }
}
