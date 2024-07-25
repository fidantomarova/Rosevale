using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rosevale.DAL;
using Rosevale.Models;

namespace Rosevale.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin, Staff")]
    public class MessagesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEmailSender _emailSender;
        public MessagesController(AppDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }
        #region Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.Messages.OrderByDescending(x => x.Id).ToListAsync());
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }

            Message? message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);

            if (message == null)
            {
                return RedirectToAction("BadRequest");
            }

            return View(message);
        }
        #endregion

        #region SendEmail
        public async Task<IActionResult> SendEmail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }
            Message? dbMessage = await _context.Messages.FirstOrDefaultAsync(x => x.Id == id);
            if (dbMessage == null)
            {
                return RedirectToAction("NotFound");
            }
            return View(dbMessage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmail(int id, Message message1, string mainMail, string email, string subject, string message)
        {
            if (id != message1.Id)
            {
                return RedirectToAction("NotFound");
            }
            Message? dbMessage = await _context.Messages.FirstOrDefaultAsync(x => x.Id == id);
            if (dbMessage == null)
            {
                return RedirectToAction("BadRequest");
            }

            mainMail = "fidan.tomarova@gmail.com";
            email = Request.Form["email"];
            subject = Request.Form["subject"];
            message = Request.Form["messageDescription"];

            await _emailSender.SendEmailAsync(mainMail, email, subject, message);

            dbMessage.IsReplied = true;
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
