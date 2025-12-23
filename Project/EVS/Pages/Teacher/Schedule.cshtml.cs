using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Teacher
{
    public class ScheduleModel : PageModel
    {
        private readonly ScheduleService _scheduleService;

        public ScheduleModel(ScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        public DataTable Schedule { get; set; } = new DataTable();

        public async Task<IActionResult> OnGetAsync()
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
            if (!teacherId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Schedule = await _scheduleService.GetScheduleAsync();

            return Page();
        }
    }
}