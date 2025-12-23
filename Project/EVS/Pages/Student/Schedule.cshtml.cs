using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EVS.Data;
using EVS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text;

namespace EVS.Pages.Student
{
    public class ScheduleModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ScheduleModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ClassSession> Sessions { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Range { get; set; } = "week";
        [BindProperty(SupportsGet = true)]
        public string Date { get; set; } = null;

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var refDate = string.IsNullOrEmpty(Date) ? System.DateTime.Today : System.DateTime.Parse(Date);
            if (Range == "day")
            {
                Sessions = await _context.ClassSessions
                    .Where(s => s.UserId == userId && s.StartTime.Date == refDate.Date)
                    .OrderBy(s => s.StartTime)
                    .ToListAsync();
            }
            else // week
            {
                var startOfWeek = refDate.AddDays(-(int)refDate.DayOfWeek + (int)DayOfWeek.Monday);
                var endOfWeek = startOfWeek.AddDays(7);
                Sessions = await _context.ClassSessions
                    .Where(s => s.UserId == userId && s.StartTime >= startOfWeek && s.StartTime < endOfWeek)
                    .OrderBy(s => s.StartTime)
                    .ToListAsync();
            }
        }

        public async Task<IActionResult> OnGetCsvAsync(string range, string date)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var refDate = string.IsNullOrEmpty(date) ? System.DateTime.Today : System.DateTime.Parse(date);
            List<ClassSession> sessions;
            if (range == "day")
            {
                sessions = await _context.ClassSessions
                    .Where(s => s.UserId == userId && s.StartTime.Date == refDate.Date)
                    .OrderBy(s => s.StartTime)
                    .ToListAsync();
            }
            else
            {
                var startOfWeek = refDate.AddDays(-(int)refDate.DayOfWeek + (int)DayOfWeek.Monday);
                var endOfWeek = startOfWeek.AddDays(7);
                sessions = await _context.ClassSessions
                    .Where(s => s.UserId == userId && s.StartTime >= startOfWeek && s.StartTime < endOfWeek)
                    .OrderBy(s => s.StartTime)
                    .ToListAsync();
            }
            var sb = new StringBuilder();
            sb.AppendLine("Subject,Teacher,StartTime,EndTime,Location,Notes");
            foreach (var s in sessions)
            {
                sb.AppendLine($"{s.Subject},{s.TeacherName},{s.StartTime},{s.EndTime},{s.Location},\"{s.Notes?.Replace("\"", "''") ?? ""}\"");
            }
            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "schedule.csv");
        }

        public async Task<IActionResult> OnGetIcalAsync(string range, string date)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var refDate = string.IsNullOrEmpty(date) ? System.DateTime.Today : System.DateTime.Parse(date);
            List<ClassSession> sessions;
            if (range == "day")
            {
                sessions = await _context.ClassSessions
                    .Where(s => s.UserId == userId && s.StartTime.Date == refDate.Date)
                    .OrderBy(s => s.StartTime)
                    .ToListAsync();
            }
            else
            {
                var startOfWeek = refDate.AddDays(-(int)refDate.DayOfWeek + (int)DayOfWeek.Monday);
                var endOfWeek = startOfWeek.AddDays(7);
                sessions = await _context.ClassSessions
                    .Where(s => s.UserId == userId && s.StartTime >= startOfWeek && s.StartTime < endOfWeek)
                    .OrderBy(s => s.StartTime)
                    .ToListAsync();
            }
            var sb = new StringBuilder();
            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("PRODID:-//EVS//Schedule//EN");
            foreach (var s in sessions)
            {
                sb.AppendLine("BEGIN:VEVENT");
                sb.AppendLine($"SUMMARY:{s.Subject} - {s.TeacherName}");
                sb.AppendLine($"DTSTART:{s.StartTime.ToUniversalTime():yyyyMMddTHHmmssZ}");
                sb.AppendLine($"DTEND:{s.EndTime.ToUniversalTime():yyyyMMddTHHmmssZ}");
                sb.AppendLine($"LOCATION:{s.Location}");
                sb.AppendLine($"DESCRIPTION:{s.Notes}");
                sb.AppendLine("END:VEVENT");
            }
            sb.AppendLine("END:VCALENDAR");
            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/calendar", "schedule.ics");
        }
    }
}
