using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Text;
using EVS.Services;

namespace EVS.Pages.Teacher
{
    public class ReportsModel : PageModel
    {
        private readonly StudentService _studentService;
        private readonly TranscriptService _transcriptService;
        private readonly AttendanceService _attendanceService;

        public ReportsModel(
            StudentService studentService, 
            TranscriptService transcriptService,
            AttendanceService attendanceService)
        {
            _studentService = studentService;
            _transcriptService = transcriptService;
            _attendanceService = attendanceService;
        }

        public DataTable Students { get; set; } = new DataTable();
        public int TotalStudents { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? GradeFilter { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
            if (!teacherId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            if (!string.IsNullOrWhiteSpace(SearchTerm) || GradeFilter.HasValue)
            {
                Students = await _studentService.SearchStudentsAsync(SearchTerm ?? "", GradeFilter);
            }
            else
            {
                Students = await _studentService.GetAllStudentsAsync();
            }

            TotalStudents = await _studentService.GetTotalStudentsCountAsync();

            return Page();
        }

        public async Task<IActionResult> OnGetExportCsvAsync()
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
            if (!teacherId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            var students = await _studentService.GetAllStudentsAsync();

            var sb = new StringBuilder();
            sb.AppendLine("StudentID,FullName,GradeLevel,ParentName");

            foreach (DataRow row in students.Rows)
            {
                sb.AppendLine($"{row["StudentID"]},{row["FullName"]},{row["GradeLevel"]},{row["ParentName"]}");
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "students_report.csv");
        }
    }
}
