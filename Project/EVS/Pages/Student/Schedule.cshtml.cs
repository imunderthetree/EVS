using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Text;
using EVS.Services;

namespace EVS.Pages.Student
{
    public class ScheduleModel : PageModel
    {
        private readonly ScheduleService _scheduleService;

        public ScheduleModel(ScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        public DataTable Sessions { get; set; } = new DataTable();

        [BindProperty(SupportsGet = true)]
        public string Range { get; set; } = "week";

        public async Task<IActionResult> OnGetAsync()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (!studentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Sessions = await _scheduleService.GetStudentScheduleAsync(studentId.Value);

            return Page();
        }

        public async Task<IActionResult> OnGetCsvAsync()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (!studentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            var sessions = await _scheduleService.GetStudentScheduleAsync(studentId.Value);

            var sb = new StringBuilder();
            sb.AppendLine("DayOfWeek,Subject,Classroom,StartTime,EndTime");

            foreach (DataRow row in sessions.Rows)
            {
                sb.AppendLine($"{row["DayOfWeek"]},{row["SubjectName"]},{row["ClassroomName"]},{row["StartTime"]},{row["EndTime"]}");
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "schedule.csv");
        }
    }
}