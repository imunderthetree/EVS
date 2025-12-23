using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Admin
{
    public class ManageClassroomsModel : PageModel
    {
        private readonly ClassroomService _classroomService;

        public ManageClassroomsModel(ClassroomService classroomService)
        {
            _classroomService = classroomService;
        }

        public DataTable Classrooms { get; set; } = new DataTable();

        [BindProperty]
        public string ClassroomName { get; set; } = string.Empty;

        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (!adminId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Classrooms = await _classroomService.GetAllClassroomsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (!adminId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            if (string.IsNullOrWhiteSpace(ClassroomName))
            {
                Message = "Please enter a classroom name";
                Classrooms = await _classroomService.GetAllClassroomsAsync();
                return Page();
            }

            await _classroomService.AddClassroomAsync(ClassroomName);
            Message = "Classroom added successfully";

            ClassroomName = string.Empty;
            Classrooms = await _classroomService.GetAllClassroomsAsync();
            return Page();
        }
    }
}