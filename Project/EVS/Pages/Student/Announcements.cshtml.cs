using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EVS.Data;
using EVS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace EVS.Pages.Student
{
    public class AnnouncementsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public AnnouncementsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Announcement> Announcements { get; set; }
        public Announcement SelectedAnnouncement { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Course { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? Date { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Announcements.AsQueryable();
            if (!string.IsNullOrWhiteSpace(Course))
                query = query.Where(a => a.Course == Course);
            if (Date.HasValue)
                query = query.Where(a => a.PostedAt.Date >= Date.Value.Date);
            Announcements = await query.OrderByDescending(a => a.PostedAt).ToListAsync();
            if (Id.HasValue)
                SelectedAnnouncement = await _context.Announcements.FindAsync(Id.Value);
        }
    }
}
