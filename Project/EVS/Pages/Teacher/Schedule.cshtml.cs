using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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

        public Dictionary<string, List<DataRow>> ScheduleByDay { get; set; } = new Dictionary<string, List<DataRow>>();

        public int TotalClasses { get; set; }

        public int DaysWithClasses
        {
            get
            {
                return ScheduleByDay?.Count(kvp => kvp.Value != null && kvp.Value.Count > 0) ?? 0;
            }
        }

        public int UniqueSubjects { get; set; }

        public List<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();

        public async Task<IActionResult> OnGetAsync()
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
            if (!teacherId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Schedule = await _scheduleService.GetScheduleAsync();

            // Populate ScheduleByDay from Schedule DataTable
            if (Schedule != null && Schedule.Rows.Count > 0)
            {
                ScheduleByDay = Schedule.AsEnumerable()
                    .GroupBy(row => row["DayOfWeek"].ToString()!)
                    .ToDictionary(g => g.Key, g => g.ToList());

                TotalClasses = Schedule.Rows.Count;

                UniqueSubjects = Schedule.AsEnumerable()
                    .Select(row => row["SubjectName"].ToString())
                    .Distinct()
                    .Count();

                // Extract unique time slots
                TimeSlots = Schedule.AsEnumerable()
                    .Select(row => new TimeSlot
                    {
                        Start = row["StartTime"].ToString()!,
                        End = row["EndTime"].ToString()!
                    })
                    .GroupBy(ts => ts.Start)
                    .Select(g => g.First())
                    .OrderBy(ts => ts.Start)
                    .ToList();
            }

            return Page();
        }
    }

    public class TimeSlot
    {
        public required string Start { get; set; }
        public required string End { get; set; }
        public string StartStr => Start;
    }
}