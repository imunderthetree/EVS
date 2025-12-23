using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Teacher
{
    public class GradeExamsModel : PageModel
    {
        private readonly ExamService _examService;
        private readonly GradeService _gradeService;

        public GradeExamsModel(ExamService examService, GradeService gradeService)
        {
            _examService = examService;
            _gradeService = gradeService;
        }

        public DataTable Exams { get; set; } = new DataTable();
        public DataTable StudentGrades { get; set; } = new DataTable();

        [BindProperty(SupportsGet = true)]
        public int? SelectedExamId { get; set; }

        [BindProperty]
        public int StudentId { get; set; }

        [BindProperty]
        public int ExamId { get; set; }

        [BindProperty]
        public decimal Score { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
            if (!teacherId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Exams = await _examService.GetAllExamsAsync();

            if (SelectedExamId.HasValue)
            {
                StudentGrades = await _gradeService.GetExamGradesAsync(SelectedExamId.Value);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostGradeAsync()
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
            if (!teacherId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            if (StudentId > 0 && ExamId > 0 && Score >= 0)
            {
                await _gradeService.GradeExamAsync(StudentId, ExamId, Score);
            }

            return RedirectToPage(new { SelectedExamId = ExamId });
        }
    }
}