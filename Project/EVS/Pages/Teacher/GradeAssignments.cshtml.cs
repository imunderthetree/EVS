using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Teacher
{
    public class GradeAssignmentsModel : PageModel
    {
        private readonly AssignmentService _assignmentService;
        private readonly GradeService _gradeService;

        public GradeAssignmentsModel(AssignmentService assignmentService, GradeService gradeService)
        {
            _assignmentService = assignmentService;
            _gradeService = gradeService;
        }

        public DataTable Assignments { get; set; } = new DataTable();
        public DataTable StudentGrades { get; set; } = new DataTable();

        [BindProperty(SupportsGet = true)]
        public int? SelectedAssignmentId { get; set; }

        [BindProperty]
        public int StudentId { get; set; }

        [BindProperty]
        public int AssignmentId { get; set; }

        [BindProperty]
        public decimal Grade { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
            if (!teacherId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Assignments = await _assignmentService.GetAllAssignmentsAsync();

            if (SelectedAssignmentId.HasValue)
            {
                StudentGrades = await _gradeService.GetAssignmentGradesAsync(SelectedAssignmentId.Value);
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

            if (StudentId > 0 && AssignmentId > 0 && Grade >= 0)
            {
                await _gradeService.GradeAssignmentAsync(StudentId, AssignmentId, Grade);
            }

            return RedirectToPage(new { SelectedAssignmentId = AssignmentId });
        }
    }
}