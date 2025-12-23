using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Admin
{
    public class ManageTeachersModel : PageModel
    {
        private readonly TeacherService _teacherService;

        public ManageTeachersModel(TeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        public DataTable Teachers { get; set; } = new DataTable();

        [BindProperty]
        public string FullName { get; set; } = string.Empty;

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (!adminId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Teachers = await _teacherService.GetAllTeachersAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (!adminId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            if (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Email))
            {
                Message = "Please fill in all fields";
                Teachers = await _teacherService.GetAllTeachersAsync();
                return Page();
            }

            await _teacherService.AddTeacherAsync(FullName, Email);
            Message = "Teacher added successfully";

            // Clear form
            FullName = string.Empty;
            Email = string.Empty;

            Teachers = await _teacherService.GetAllTeachersAsync();
            return Page();
        }
    }
}