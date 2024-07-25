using Rosevale.DAL;
using Rosevale.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Message = Rosevale.Models.Message;
namespace Rosevale.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _db;
        public ContactController(AppDbContext db)
        {
            _db = db;
        }
        #region Index
        public async Task<IActionResult> Index()
        {
            return View(await _db.Messages.FirstOrDefaultAsync());
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

        public async Task<IActionResult> Create(Message message)
        {
            _db.Messages.Add(message);
            await _db.SaveChangesAsync();
            return RedirectToAction("Thanks");
        }
        #endregion
    }
}