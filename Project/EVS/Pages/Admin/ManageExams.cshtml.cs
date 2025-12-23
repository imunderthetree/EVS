using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Admin
{
    public class ManageExamsModel : PageModel
    {
        private readonly ExamService _examService;
        private readonly SubjectService _subjectService;

        public ManageExamsModel(ExamService examService, SubjectService subjectService)
        {
            _examService = examService;
            _subjectService = subjectService;
        }

        public DataTable Exams { get; set; } = new DataTable();
        public DataTable Subjects { get; set; } = new DataTable();

        public async Task<IActionResult> OnGetAsync()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (!adminId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Exams = await _examService.GetAllExamsAsync();
            Subjects = await _subjectService.GetAllSubjectsAsync();
            return Page();
        }
    }
}