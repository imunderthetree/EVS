using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Text;
using EVS.Services;
using System;
using System.Collections.Generic;

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
        public List<Message> Messages { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (!studentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Grades = await _transcriptService.GetStudentGradesAsync(studentId.Value);
            Attendance = await _attendanceService.GetStudentAttendanceAsync(studentId.Value);

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

        public void OnGet()
        {
            // Example initialization, replace with your actual data retrieval logic
            Messages = new List<Message>
            {
                new Message { SentAt = DateTime.Now, Content = "Report 1" },
                new Message { SentAt = DateTime.Now.AddDays(-1), Content = "Report 2" }
            };
        }
    }

    public class Message
    {
        public DateTime SentAt { get; set; }
        public string Content { get; set; }
    }
}