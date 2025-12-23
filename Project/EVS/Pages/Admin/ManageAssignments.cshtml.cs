using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Admin
{
    public class ManageAssignmentsModel : PageModel
    {
        private readonly AssignmentService _assignmentService;
        private readonly SubjectService _subjectService;

        public ManageAssignmentsModel(AssignmentService assignmentService, SubjectService subjectService)
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

        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (!adminId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Assignments = await _assignmentService.GetAllAssignmentsAsync();
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

            if (string.IsNullOrWhiteSpace(Title) || SubjectId <= 0 || TotalGrade <= 0)
            {
                Message = "Please fill in all fields correctly";
                Assignments = await _assignmentService.GetAllAssignmentsAsync();
                Subjects = await _subjectService.GetAllSubjectsAsync();
                return Page();
            }

            await _assignmentService.AddAssignmentAsync(Title, SubjectId, TotalGrade);
            Message = "Assignment added successfully";

            // Clear form
            Title = string.Empty;
            SubjectId = 0;
            TotalGrade = 0;

            Assignments = await _assignmentService.GetAllAssignmentsAsync();
            Subjects = await _subjectService.GetAllSubjectsAsync();
            return Page();
        }
    }
}