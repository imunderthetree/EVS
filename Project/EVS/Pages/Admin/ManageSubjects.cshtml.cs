using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Admin
{
    public class ManageSubjectsModel : PageModel
    {
        private readonly SubjectService _subjectService;

        public ManageSubjectsModel(SubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        public DataTable Subjects { get; set; } = new DataTable();

        [BindProperty]
        public string SubjectName { get; set; } = string.Empty;

        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (!adminId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Subjects = await _subjectService.GetAllSubjectsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (!adminId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            if (string.IsNullOrWhiteSpace(SubjectName))
            {
                Message = "Please enter a subject name";
                Subjects = await _subjectService.GetAllSubjectsAsync();
                return Page();
            }

            await _subjectService.AddSubjectAsync(SubjectName);
            Message = "Subject added successfully";

            SubjectName = string.Empty;
            Subjects = await _subjectService.GetAllSubjectsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (!adminId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            await _subjectService.DeleteSubjectAsync(id);
            return RedirectToPage();
        }
    }
}