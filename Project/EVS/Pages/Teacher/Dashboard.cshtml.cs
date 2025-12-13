using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EVS.Pages.Teacher
{
    public class DashboardModel : PageModel
    {
        [BindProperty]
        public string AnnouncementSubject { get; set; } = string.Empty;

        [BindProperty]
        public string AnnouncementContent { get; set; } = string.Empty;

        public IEnumerable<SelectListItem> Subjects { get; set; } = Enumerable.Empty<SelectListItem>();

        public string? AnnouncementResult { get; set; }

        public void OnGet()
        {
            PopulateSubjects();

            if (TempData.ContainsKey("AnnouncementResult"))
            {
                AnnouncementResult = TempData["AnnouncementResult"]?.ToString();
            }
        }

        public IActionResult OnPostCreateAnnouncement()
        {
            PopulateSubjects();

            if (string.IsNullOrWhiteSpace(AnnouncementSubject))
            {
                ModelState.AddModelError(nameof(AnnouncementSubject), "Please select a subject.");
            }

            if (string.IsNullOrWhiteSpace(AnnouncementContent))
            {
                ModelState.AddModelError(nameof(AnnouncementContent), "Please enter an announcement.");
            }

            if (!ModelState.IsValid)
            {
                // Return page to show validation errors and re-open modal client-side
                return Page();
            }

            // TODO: persist announcement or push notifications via your data layer/service
            // For now we set TempData so the UI can show a confirmation after PRG redirect.
            TempData["AnnouncementResult"] = $"Announcement sent to {AnnouncementSubject}.";

            return RedirectToPage();
        }

        private void PopulateSubjects()
        {
            // Replace with real subject list from your data access layer.
            Subjects = new List<SelectListItem>
            {
                new SelectListItem("Mathematics", "Mathematics"),
                new SelectListItem("English", "English"),
                new SelectListItem("Science", "Science"),
                new SelectListItem("History", "History"),
                new SelectListItem("Art", "Art")
            };
        }
    }
}
