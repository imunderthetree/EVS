using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Student
{
    public class AssignmentsModel : PageModel
    {
        private readonly AssignmentService _assignmentService;
        private readonly GradeService _gradeService;

        public AssignmentsModel(AssignmentService assignmentService, GradeService gradeService)
        {
            _assignmentService = assignmentService;
            _gradeService = gradeService;
        }

        public DataTable Assignments { get; set; } = new DataTable();
        public Dictionary<int, (decimal Grade, decimal TotalGrade)> AssignmentGrades { get; set; } = new();
        public string StudentName { get; set; } = "Student";

        public async Task<IActionResult> OnGetAsync()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (!studentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            StudentName = HttpContext.Session.GetString("StudentName") ?? "Student";
            Assignments = await _assignmentService.GetAllAssignmentsAsync();

            // Load student's assignment grades
            foreach (DataRow row in Assignments.Rows)
            {
                var assignmentId = Convert.ToInt32(row["AssignmentID"]);
                var totalGrade = Convert.ToDecimal(row["TotalGrade"]);
                var grades = await _gradeService.GetAssignmentGradesAsync(assignmentId);

                foreach (DataRow gradeRow in grades.Rows)
                {
                    if (Convert.ToInt32(gradeRow["StudentID"]) == studentId.Value)
                    {
                        var grade = Convert.ToDecimal(gradeRow["Grade"]);
                        AssignmentGrades[assignmentId] = (grade, totalGrade);
                        break;
                    }
                }
            }

            return Page();
        }

        public decimal CalculateOverallProgress()
        {
            if (Assignments.Rows.Count == 0) return 0;
            return (decimal)AssignmentGrades.Count / Assignments.Rows.Count * 100;
        }
    }
}