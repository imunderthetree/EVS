using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Text;
using EVS.Services;

namespace EVS.Pages.Student
{
    public class TranscriptModel : PageModel
    {
        private readonly TranscriptService _transcriptService;

        public TranscriptModel(TranscriptService transcriptService)
        {
            _transcriptService = transcriptService;
        }

        public DataTable Grades { get; set; } = new DataTable();
        public double CumulativeGPA { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (!studentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Grades = await _transcriptService.GetStudentGradesAsync(studentId.Value);

            // Calculate GPA (simple average for demonstration)
            if (Grades.Rows.Count > 0)
            {
                decimal totalGrade = 0;
                int count = 0;

                foreach (DataRow row in Grades.Rows)
                {
                    if (row["Grade"] != DBNull.Value)
                    {
                        totalGrade += Convert.ToDecimal(row["Grade"]);
                        count++;
                    }
                }

                CumulativeGPA = count > 0 ? (double)(totalGrade / count) : 0;
            }

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

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "transcript.csv");
        }

        public async Task<IActionResult> OnGetPdfAsync()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (!studentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            var grades = await _transcriptService.GetStudentGradesAsync(studentId.Value);

            // Simple PDF placeholder
            var pdfBytes = Encoding.UTF8.GetBytes($"PDF generation not implemented. Total grades: {grades.Rows.Count}");
            return File(pdfBytes, "application/pdf", "transcript.pdf");
        }
    }
}