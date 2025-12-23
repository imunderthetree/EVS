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
                var students = await _studentService.GetAllStudentsAsync();
                var student = students.AsEnumerable()
                    .FirstOrDefault(r => Convert.ToInt32(r["StudentID"]) == StudentId);

                if (student != null)
                {
                    StudentInfo = student;
                }
            }

            return Page();
        }
    }
}
