using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Teacher
{
    public class DashboardModel : PageModel
    {
        private readonly AnnouncementService _announcementService;
        private readonly MessageService _messageService;

        public DashboardModel(AnnouncementService announcementService, MessageService messageService)
        {
            _announcementService = announcementService;
            _messageService = messageService;
        }

        public DataTable RecentMessages { get; set; } = new DataTable();

        [BindProperty]
        public string AnnouncementTitle { get; set; } = string.Empty;

        [BindProperty]
        public string AnnouncementContent { get; set; } = string.Empty;

        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
            if (!teacherId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            RecentMessages = await _messageService.GetUserMessagesAsync(teacherId.Value);

            return Page();
        }

        public async Task<IActionResult> OnPostCreateAnnouncementAsync()
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
            if (!teacherId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            if (string.IsNullOrWhiteSpace(AnnouncementTitle) || string.IsNullOrWhiteSpace(AnnouncementContent))
            {
                Message = "Please fill in all fields";
                return Page();
            }

            await _announcementService.CreateAnnouncementAsync(AnnouncementTitle, AnnouncementContent);
            Message = "Announcement created successfully";

            return RedirectToPage();
        }
    }
}