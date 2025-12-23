using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EVS.Data;
using EVS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace EVS.Pages.Student
{
    public class AttendanceModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public AttendanceModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<AttendanceRecord> Records { get; set; }
        public Dictionary<string, (int Present, int Absent, int Late, int Total)> Summary { get; set; }
        [BindProperty]
        public int? JustifyId { get; set; }
        [BindProperty]
        public string Justification { get; set; }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Records = await _context.AttendanceRecords
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Date)
                .ToListAsync();
            Summary = Records
                .GroupBy(r => r.ClassName)
                .ToDictionary(
                    g => g.Key,
                    g => (
                        Present: g.Count(r => r.Status == "Present"),
                        Absent: g.Count(r => r.Status == "Absent"),
                        Late: g.Count(r => r.Status == "Late"),
                        Total: g.Count()
                    )
                );
        }

        public async Task<IActionResult> OnPostJustifyAsync()
        {
            if (JustifyId.HasValue && !string.IsNullOrWhiteSpace(Justification))
            {
                var record = await _context.AttendanceRecords.FindAsync(JustifyId.Value);
                if (record != null)
                {
                    record.Justification = Justification;
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToPage();
        }
    }
}