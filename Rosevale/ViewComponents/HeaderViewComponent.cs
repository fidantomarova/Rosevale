using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rosevale.DAL;
using Rosevale.Models;
using Rosevale.ViewModels;

namespace Rosevale.ViewComponents
{
    public class HeaderViewComponent: ViewComponent
    {
        private readonly AppDbContext _db;
        public HeaderViewComponent(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            HeaderVM headerVM = new HeaderVM
            {
                Bios = await _db.Bios.FirstOrDefaultAsync(),
                SocialMedias = await _db.SocialMedias.ToListAsync(),
            };
            return View(headerVM);
        }
    }
}
