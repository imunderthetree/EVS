using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EVS.Services;

namespace EVS.Pages.Student
{
    public class ClassroomsModel : PageModel
    {
        private readonly ClassroomService _classroomService;
        private readonly ScheduleService _scheduleService;

        public ClassroomsModel(ClassroomService classroomService, ScheduleService scheduleService)
        {
            _classroomService = classroomService;
            _scheduleService = scheduleService;
        }

        public DataTable Classrooms { get; set; } = new DataTable();
        public DataTable Schedule { get; set; } = new DataTable();
        public string StudentName { get; set; } = "Student";
        public int TotalClassrooms { get; set; }
        public Dictionary<string, List<ScheduleItem>> ScheduleByDay { get; set; } = new Dictionary<string, List<ScheduleItem>>();

        public async Task<IActionResult> OnGetAsync()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (!studentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            StudentName = HttpContext.Session.GetString("AdminName") ?? "Student";

            try
            {
                // TODO: Replace with actual classroom service call once ambiguity is resolved
                // Classrooms = await _classroomService.GetAllClassroomsAsync();
                Classrooms = new DataTable();
                TotalClassrooms = Classrooms.Rows.Count;

                // Get student's schedule
                Schedule = await _scheduleService.GetStudentScheduleAsync(studentId.Value);

                // Organize schedule by day
                ScheduleByDay = OrganizeScheduleByDay(Schedule);
            }
            catch (Exception ex)
            {
                // Log error - in production, use ILogger
                Console.WriteLine($"Error loading classrooms: {ex.Message}");

                // Initialize empty data to prevent errors
                Classrooms = new DataTable();
                Schedule = new DataTable();
            }

            return Page();
        }

        private Dictionary<string, List<ScheduleItem>> OrganizeScheduleByDay(DataTable schedule)
        {
            var result = new Dictionary<string, List<ScheduleItem>>
            {
                { "Monday", new List<ScheduleItem>() },
                { "Tuesday", new List<ScheduleItem>() },
                { "Wednesday", new List<ScheduleItem>() },
                { "Thursday", new List<ScheduleItem>() },
                { "Friday", new List<ScheduleItem>() }
            };

            if (schedule == null || schedule.Rows.Count == 0)
                return result;

            foreach (DataRow row in schedule.Rows)
            {
                var day = row["DayOfWeek"].ToString();
                if (!string.IsNullOrEmpty(day) && result.ContainsKey(day))
                {
                    result[day].Add(new ScheduleItem
                    {
                        SubjectName = row["SubjectName"]?.ToString() ?? "N/A",
                        ClassroomName = row["ClassroomName"]?.ToString() ?? "N/A",
                        StartTime = row["StartTime"]?.ToString() ?? "N/A",
                        EndTime = row["EndTime"]?.ToString() ?? "N/A"
                    });
                }
            }

            return result;
        }

        public class ScheduleItem
        {
            public string SubjectName { get; set; } = string.Empty;
            public string ClassroomName { get; set; } = string.Empty;
            public string StartTime { get; set; } = string.Empty;
            public string EndTime { get; set; } = string.Empty;
        }
    }
}