using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Student
{
    public class ClassroomDetailsModel : PageModel
    {
        private readonly ClassroomService _classroomService;

        public ClassroomDetailsModel(ClassroomService classroomService)
        {
            _classroomService = classroomService;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public string ClassroomName { get; set; } = string.Empty;
        public DataTable Schedule { get; set; } = new DataTable();
        public DataTable Exams { get; set; } = new DataTable();
        public DataTable Assignments { get; set; } = new DataTable();

        public async Task<IActionResult> OnGetAsync()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (!studentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            if (Id <= 0)
            {
                return RedirectToPage("/Student/Classrooms");
            }

            var classrooms = await _classroomService.GetAllClassroomsAsync();
            var classroom = classrooms.AsEnumerable().FirstOrDefault(r => r.Field<int>("ClassroomId") == Id);
            if (classroom == null)
            {
                return RedirectToPage("/Student/Classrooms");
            }

            ClassroomName = classroom["ClassroomName"]?.ToString() ?? "Classroom";
            Schedule = await _classroomService.GetClassroomScheduleAsync(Id);
            Exams = new DataTable();
            Assignments = await _classroomService.GetClassroomAssignmentsAsync(Id);

            return Page();
        }
    }
}