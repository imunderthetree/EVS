using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Teacher
{
    public class AssignmentsModel : PageModel
    {
        private readonly AssignmentService _assignmentService;
        private readonly SubjectService _subjectService;

        public AssignmentsModel(AssignmentService assignmentService, SubjectService subjectService)
        {
            _assignmentService = assignmentService;
            _subjectService = subjectService;
        }

        public DataTable Assignments { get; set; } = new DataTable();
        public DataTable Subjects { get; set; } = new DataTable();

        [BindProperty]
        public string Title { get; set; } = string.Empty;

        [BindProperty]
        public int SubjectId { get; set; }

        [BindProperty]
        public decimal TotalGrade { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
            if (!teacherId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Assignments = await _assignmentService.GetAllAssignmentsAsync();
            Subjects = await _subjectService.GetAllSubjectsAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
            if (!teacherId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            if (string.IsNullOrWhiteSpace(Title) || SubjectId <= 0 || TotalGrade <= 0)
            {
                Assignments = await _assignmentService.GetAllAssignmentsAsync();
                Subjects = await _subjectService.GetAllSubjectsAsync();
                return Page();
            }

            await _assignmentService.AddAssignmentAsync(Title, SubjectId, TotalGrade);

            return RedirectToPage();
        }
    }
}