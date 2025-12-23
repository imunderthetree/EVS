using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Student
{
    public class AttendanceModel : PageModel
    {
        private readonly AttendanceService _attendanceService;

        public AttendanceModel(AttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        public DataTable Records { get; set; } = new DataTable();
        public Dictionary<string, int> Summary { get; set; } = new Dictionary<string, int>();

        public async Task<IActionResult> OnGetAsync()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (!studentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Records = await _attendanceService.GetStudentAttendanceAsync(studentId.Value);
            Summary = await _attendanceService.GetAttendanceSummaryAsync(studentId.Value);

            return Page();
        }
    }
}