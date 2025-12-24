using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Admin
{
    public class ScheduleManagementModel : PageModel
    {
        private readonly ScheduleService _scheduleService;
        private readonly SubjectService _subjectService;

        public ScheduleManagementModel(ScheduleService scheduleService, SubjectService subjectService)
        {
            _scheduleService = scheduleService;
            _subjectService = subjectService;
        }

        public DataTable Schedule { get; set; } = new DataTable();
        public DataTable Subjects { get; set; } = new DataTable();

        public async Task<IActionResult> OnGetAsync()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (!adminId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Schedule = await _scheduleService.GetScheduleAsync();
            Subjects = await _subjectService.GetAllSubjectsAsync();

            return Page();
        }
    }
}