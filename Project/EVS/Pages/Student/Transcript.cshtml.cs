using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EVS.Data;
using EVS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text;

namespace EVS.Pages.Student
{
    public class TranscriptModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public TranscriptModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<CourseGrade> Grades { get; set; }
        public double CumulativeGPA { get; set; }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Grades = await _context.CourseGrades
                .Where(g => g.UserId == userId)
                .OrderBy(g => g.Term)
                .ThenBy(g => g.CourseName)
                .ToListAsync();
            if (Grades.Count > 0)
            {
                var totalPoints = Grades.Sum(g => g.GradePoint * g.Credits);
                var totalCredits = Grades.Sum(g => g.Credits);
                CumulativeGPA = totalCredits > 0 ? totalPoints / totalCredits : 0;
            }
        }

        public async Task<IActionResult> OnGetCsvAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var grades = await _context.CourseGrades
                .Where(g => g.UserId == userId)
                .OrderBy(g => g.Term)
                .ThenBy(g => g.CourseName)
                .ToListAsync();
            var sb = new StringBuilder();
            sb.AppendLine("Term,Course,Grade,GradePoint,Credits");
            foreach (var g in grades)
            {
                sb.AppendLine($"{g.Term},{g.CourseName},{g.Grade},{g.GradePoint},{g.Credits}");
            }
            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "transcript.csv");
        }

        public async Task<IActionResult> OnGetPdfAsync()
        {
            // For demonstration, generate a simple PDF using QuestPDF or similar (pseudo-code)
            // In real use, add QuestPDF or another library and implement PDF generation
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var grades = await _context.CourseGrades
                .Where(g => g.UserId == userId)
                .OrderBy(g => g.Term)
                .ThenBy(g => g.CourseName)
                .ToListAsync();
            // TODO: Replace with real PDF generation
            var pdfBytes = Encoding.UTF8.GetBytes("PDF generation not implemented.\nCourses: " + grades.Count);
            return File(pdfBytes, "application/pdf", "transcript.pdf");
        }
    }
}
