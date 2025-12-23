using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Teacher
{
    public class MySubjectsModel : PageModel
    {
        private readonly SubjectService _subjectService;

        public MySubjectsModel(SubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        public DataTable Subjects { get; set; } = new DataTable();

        public async Task<IActionResult> OnGetAsync()
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
            if (!teacherId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Subjects = await _subjectService.GetAllSubjectsAsync();

            return Page();
        }
    }
}