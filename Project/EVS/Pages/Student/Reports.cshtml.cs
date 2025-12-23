using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Text;
using EVS.Services;

namespace EVS.Pages.Student
{
    public class ReportsModel : PageModel
    {
        private readonly TranscriptService _transcriptService;
        private readonly AttendanceService _attendanceService;

        public ReportsModel(TranscriptService transcriptService, AttendanceService attendanceService)
        {
            _transcriptService = transcriptService;
            _attendanceService = attendanceService;
        }

        public DataTable Grades { get; set; } = new DataTable();
        public DataTable Attendance { get; set; } = new DataTable();
        public string StudentName { get; set; } = "Student";
        public int AttendancePresent { get; set; }
        public int AttendanceAbsent { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (!studentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            StudentName = HttpContext.Session.GetString("AdminName") ?? "Student";
            Grades = await _transcriptService.GetStudentGradesAsync(studentId.Value);
            Attendance = await _attendanceService.GetStudentAttendanceAsync(studentId.Value);

            // Calculate attendance summary
            var summary = await _attendanceService.GetAttendanceSummaryAsync(studentId.Value);
            AttendancePresent = summary.GetValueOrDefault("Present", 0);
            AttendanceAbsent = summary.GetValueOrDefault("Absent", 0);

            return Page();
        }

        public async Task<IActionResult> OnGetCsvAsync()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (!studentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            var grades = await _transcriptService.GetStudentGradesAsync(studentId.Value);

            var sb = new StringBuilder();
            sb.AppendLine("Subject,Assignment,Grade");

            foreach (DataRow row in grades.Rows)
            {
                sb.AppendLine($"{row["SubjectName"]},{row["AssignmentTitle"]},{row["Grade"]}");
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "report.csv");
        }
    }
}