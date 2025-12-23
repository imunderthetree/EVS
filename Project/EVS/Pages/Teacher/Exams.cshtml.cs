using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Teacher
{
    public class ExamsModel : PageModel
    {
        private readonly ExamService _examService;
        private readonly SubjectService _subjectService;

        public ExamsModel(ExamService examService, SubjectService subjectService)
        {
            _examService = examService;
            _subjectService = subjectService;
        }

        public DataTable Exams { get; set; } = new DataTable();
        public DataTable Subjects { get; set; } = new DataTable();

        public async Task<IActionResult> OnGetAsync()
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
            if (!teacherId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Exams = await _examService.GetAllExamsAsync();
            Subjects = await _subjectService.GetAllSubjectsAsync();

            return Page();
        }
    }
}