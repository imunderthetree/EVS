using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Parent
{
    public class AnnouncementsModel : PageModel
    {
        private readonly AnnouncementService _announcementService;

        public AnnouncementsModel(AnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        public DataTable Announcements { get; set; } = new DataTable();

        public async Task<IActionResult> OnGetAsync()
        {
            var parentId = HttpContext.Session.GetInt32("ParentId");
            if (!parentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Announcements = await _announcementService.GetAllAnnouncementsAsync();

            return Page();
        }
    }
}
