using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Parent
{
    public class AttendanceModel : PageModel
    {
        private readonly AttendanceService _attendanceService;

        public AttendanceModel(AttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        public DataTable AttendanceRecords { get; set; } = new DataTable();
        public Dictionary<string, int> Summary { get; set; } = new Dictionary<string, int>();

        [BindProperty(SupportsGet = true)]
        public int StudentId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var parentId = HttpContext.Session.GetInt32("ParentId");
            if (!parentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            if (StudentId > 0)
            {
                AttendanceRecords = await _attendanceService.GetStudentAttendanceAsync(StudentId);
                Summary = await _attendanceService.GetAttendanceSummaryAsync(StudentId);
            }

            return Page();
        }
    }
}