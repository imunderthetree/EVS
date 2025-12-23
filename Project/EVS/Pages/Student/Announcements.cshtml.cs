using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Student
{
    public class AnnouncementsModel : PageModel
    {
        private readonly AnnouncementService _announcementService;

        public AnnouncementsModel(AnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        public DataTable Announcements { get; set; } = new DataTable();
        public DataRow? SelectedAnnouncement { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; }

        // Add this property to support the filter input in the .cshtml file
        [BindProperty(SupportsGet = true)]
        public string? Course { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (!studentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Announcements = await _announcementService.GetAllAnnouncementsAsync();

            if (Id.HasValue)
            {
                var announcement = await _announcementService.GetAnnouncementByIdAsync(Id.Value);
                if (announcement.Rows.Count > 0)
                {
                    SelectedAnnouncement = announcement.Rows[0];
                }
            }

            return Page();
        }
    }
}
