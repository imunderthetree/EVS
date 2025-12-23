using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EVS.Services;

namespace EVS.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly StudentService _studentService;
        private readonly TeacherService _teacherService;
        private readonly ParentService _parentService;
        private readonly SubjectService _subjectService;

        public DashboardModel(
            StudentService studentService,
            TeacherService teacherService,
            ParentService parentService,
            SubjectService subjectService)
        {
            _studentService = studentService;
            _teacherService = teacherService;
            _parentService = parentService;
            _subjectService = subjectService;
        }

        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalParents { get; set; }
        public int TotalSubjects { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Check if user is logged in
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (!adminId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            // Load dashboard statistics
            TotalStudents = await _studentService.GetTotalStudentsCountAsync();
            TotalTeachers = await _teacherService.GetTotalTeachersCountAsync();
            TotalParents = await _parentService.GetTotalParentsCountAsync();
            TotalSubjects = await _subjectService.GetTotalSubjectsCountAsync();

            return Page();
        }
    }
}