using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Parent
{
    public class DashboardModel : PageModel
    {
        private readonly StudentService _studentService;
        private readonly AnnouncementService _announcementService;

        public DashboardModel(
            StudentService studentService,
            AnnouncementService announcementService)
        {
            _studentService = studentService;
            _announcementService = announcementService;
        }

        public DataTable Children { get; set; } = new DataTable();
        public DataTable RecentAnnouncements { get; set; } = new DataTable();
        public string ParentName { get; set; } = "Parent";

        public async Task<IActionResult> OnGetAsync()
        {
            var parentId = HttpContext.Session.GetInt32("ParentId");
            if (!parentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            ParentName = HttpContext.Session.GetString("AdminName") ?? "Parent";

            // Get parent's children
            Children = await _studentService.GetStudentsByParentAsync(parentId.Value);

            // Get recent announcements
            var announcements = await _announcementService.GetAllAnnouncementsAsync();
            var recentRows = announcements.AsEnumerable().Take(3);
            if (recentRows.Any())
            {
                RecentAnnouncements = recentRows.CopyToDataTable();
            }
            else
            {
                RecentAnnouncements = announcements.Clone();
            }

            return Page();
        }
    }
}