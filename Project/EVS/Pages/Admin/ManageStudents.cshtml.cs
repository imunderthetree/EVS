using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Admin
{
    public class ManageStudentsModel : PageModel
    {
        private readonly StudentService _studentService;

        public ManageStudentsModel(StudentService studentService)
        {
            _studentService = studentService;
        }

        public DataTable Students { get; set; } = new DataTable();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? GradeFilter { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (!adminId.HasValue)
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

            return Page();
        }
    }
}