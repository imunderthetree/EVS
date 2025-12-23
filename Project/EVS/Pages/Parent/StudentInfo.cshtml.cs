using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Parent
{
    public class StudentInfoModel : PageModel
    {
        private readonly StudentService _studentService;

        public StudentInfoModel(StudentService studentService)
        {
            _studentService = studentService;
        }

        public DataRow? StudentInfo { get; set; }
        public DataTable Children { get; set; } = new DataTable();

        [BindProperty(SupportsGet = true)]
        public int StudentId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var parentId = HttpContext.Session.GetInt32("ParentId");
            if (!parentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            // Get all children for this parent
            Children = await _studentService.GetStudentsByParentAsync(parentId.Value);

            if (StudentId > 0)
            {
                // Verify this student belongs to this parent
                var student = Children.AsEnumerable()
                    .FirstOrDefault(r => Convert.ToInt32(r["StudentID"]) == StudentId);

                if (student != null)
                {
                    StudentInfo = student;
                }
            }
            else if (Children.Rows.Count > 0)
            {
                // Default to first child if none selected
                StudentInfo = Children.Rows[0];
                StudentId = Convert.ToInt32(StudentInfo["StudentID"]);
            }

            return Page();
        }
    }
}
