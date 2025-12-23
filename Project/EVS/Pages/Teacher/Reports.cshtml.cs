using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Teacher
{
    public class ReportsModel : PageModel
    {
        private readonly StudentService _studentService;
        private readonly TranscriptService _transcriptService;

        public ReportsModel(StudentService studentService, TranscriptService transcriptService)
        {
            _studentService = studentService;
            _transcriptService = transcriptService;
        }

        public DataTable Students { get; set; } = new DataTable();

        public async Task<IActionResult> OnGetAsync()
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
            if (!teacherId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Students = await _studentService.GetAllStudentsAsync();

            return Page();
        }
    }
}
