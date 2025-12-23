using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Parent
{
    public class TranscriptModel : PageModel
    {
        private readonly TranscriptService _transcriptService;
        private readonly StudentService _studentService;

        public TranscriptModel(TranscriptService transcriptService, StudentService studentService)
        {
            _transcriptService = transcriptService;
            _studentService = studentService;
        }

        public DataTable Grades { get; set; } = new DataTable();
        public DataTable Children { get; set; } = new DataTable();
        public double CumulativeGPA { get; set; }

        [BindProperty(SupportsGet = true)]
        public int StudentId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var parentId = HttpContext.Session.GetInt32("ParentId");
            if (!parentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            // Get parent's children
            Children = await _studentService.GetStudentsByParentAsync(parentId.Value);

            if (StudentId > 0)
            {
                // Verify this student belongs to this parent
                var isValidChild = Children.AsEnumerable()
                    .Any(r => Convert.ToInt32(r["StudentID"]) == StudentId);

                if (isValidChild)
                {
                    Grades = await _transcriptService.GetStudentGradesAsync(StudentId);

                    // Calculate GPA
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
                }
            }

            return Page();
        }
    }
}