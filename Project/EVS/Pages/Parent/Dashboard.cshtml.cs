using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Parent
{
    public class DashboardModel : PageModel
    {
        private readonly ParentService _parentService;
        private readonly StudentService _studentService;
        private readonly AnnouncementService _announcementService;

        public DashboardModel(
            ParentService parentService,
            StudentService studentService,
            AnnouncementService announcementService)
        {
            _parentService = parentService;
            _studentService = studentService;
            _announcementService = announcementService;
        }

        public DataTable Children { get; set; } = new DataTable();
        public DataTable RecentAnnouncements { get; set; } = new DataTable();

        public async Task<IActionResult> OnGetAsync()
        {
            var parentId = HttpContext.Session.GetInt32("ParentId");
            if (!parentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            // Get parent's children
            var query = @"SELECT StudentID, FullName, GradeLevel 
                         FROM Student 
                         WHERE ParentID = @parentId";

            var parameters = new Dictionary<string, object> { { "@parentId", parentId.Value } };

            // Using a temporary service call approach
            var students = await _studentService.SearchStudentsAsync("", null);
            Children = students.AsEnumerable()
                .Where(r => Convert.ToInt32(r["ParentID"]) == parentId.Value)
                .CopyToDataTable();

            // Get recent announcements
            var announcements = await _announcementService.GetAllAnnouncementsAsync();
            RecentAnnouncements = announcements.AsEnumerable().Take(3).CopyToDataTable();

            return Page();
        }
    }
}