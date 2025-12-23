using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Student
{
    public class ExamsModel : PageModel
    {
        private readonly ExamService _examService;
        private readonly GradeService _gradeService;

        public ExamsModel(ExamService examService, GradeService gradeService)
        {
            _examService = examService;
            _gradeService = gradeService;
        }

        public DataTable Exams { get; set; } = new DataTable();
        public Dictionary<int, decimal?> ExamScores { get; set; } = new();
        public string StudentName { get; set; } = "Student";

        public async Task<IActionResult> OnGetAsync()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (!studentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            StudentName = HttpContext.Session.GetString("StudentName") ?? "Student";
            Exams = await _examService.GetAllExamsAsync();

            // Load student's exam scores
            foreach (DataRow row in Exams.Rows)
            {
                var examId = Convert.ToInt32(row["ExamID"]);
                var grades = await _gradeService.GetExamGradesAsync(examId);
                
                foreach (DataRow gradeRow in grades.Rows)
                {
                    if (Convert.ToInt32(gradeRow["StudentID"]) == studentId.Value)
                    {
                        ExamScores[examId] = Convert.ToDecimal(gradeRow["Score"]);
                        break;
                    }
                }
            }

            return Page();
        }
    }
}