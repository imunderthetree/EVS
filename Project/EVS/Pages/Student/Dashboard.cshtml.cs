using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Student
{
    public class DashboardModel : PageModel
    {
        private readonly AnnouncementService _announcementService;
        private readonly AttendanceService _attendanceService;

        public DashboardModel(AnnouncementService announcementService, AttendanceService attendanceService)
        {
            _announcementService = announcementService;
            _attendanceService = attendanceService;
        }

        public DataTable RecentAnnouncements { get; set; } = new DataTable();
        public Dictionary<string, int> AttendanceSummary { get; set; } = new Dictionary<string, int>();

        public async Task<IActionResult> OnGetAsync()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (!studentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            // Get recent announcements
            var announcements = await _announcementService.GetAllAnnouncementsAsync();
            RecentAnnouncements = announcements.AsEnumerable().Take(5).CopyToDataTable();

            // Get attendance summary
            AttendanceSummary = await _attendanceService.GetAttendanceSummaryAsync(studentId.Value);

            return Page();
        }
    }
}