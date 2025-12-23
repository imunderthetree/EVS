using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Teacher
{
    public class DashboardModel : PageModel
    {
        private readonly AnnouncementService _announcementService;
        private readonly MessageService _messageService;
        private readonly ScheduleService _scheduleService;
        private readonly AssignmentService _assignmentService;
        private readonly SubjectService _subjectService;

        public DashboardModel(
            AnnouncementService announcementService,
            MessageService messageService,
            ScheduleService scheduleService,
            AssignmentService assignmentService,
            SubjectService subjectService)
        {
            _announcementService = announcementService;
            _messageService = messageService;
            _scheduleService = scheduleService;
            _assignmentService = assignmentService;
            _subjectService = subjectService;
        }

        public DataTable RecentMessages { get; set; } = new DataTable();
        public DataTable RecentAnnouncements { get; set; } = new DataTable();
        public DataTable TodaySchedule { get; set; } = new DataTable();

        public int TodayClassesCount { get; set; }
        public int PendingAssignmentsCount { get; set; }
        public int SubjectCount { get; set; }
        public string TeacherName { get; set; } = "Teacher";
        public int TeacherId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Title is required")]
        public string AnnouncementTitle { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Content is required")]
        public string AnnouncementContent { get; set; } = string.Empty;

        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
            if (!teacherId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            TeacherId = teacherId.Value;
            TeacherName = HttpContext.Session.GetString("AdminName") ?? "Teacher";

            // Load dashboard data
            RecentMessages = await _messageService.GetUserMessagesAsync(teacherId.Value);
            RecentAnnouncements = await _announcementService.GetAllAnnouncementsAsync();

            // Get today's schedule
            var allSchedule = await _scheduleService.GetScheduleAsync();
            var today = DateTime.Now.DayOfWeek.ToString();

            var todayRows = allSchedule.AsEnumerable()
                .Where(row => row["DayOfWeek"].ToString() == today);

            if (todayRows.Any())
            {
                TodaySchedule = todayRows.CopyToDataTable();
            }
            else
            {
                TodaySchedule = allSchedule.Clone(); // Creates empty table with same schema
            }

            TodayClassesCount = TodaySchedule.Rows.Count;

            // Get pending assignments count (assignments without grades)
            var assignments = await _assignmentService.GetAllAssignmentsAsync();
            PendingAssignmentsCount = assignments.Rows.Count;

            // Get subject count
            var subjects = await _subjectService.GetAllSubjectsAsync();
            SubjectCount = subjects.Rows.Count;

            return Page();
        }

        public async Task<IActionResult> OnPostCreateAnnouncementAsync()
        {
            var teacherId = HttpContext.Session.GetInt32("TeacherId");
            if (!teacherId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            if (!ModelState.IsValid)
            {
                Message = "Please fill in all fields correctly";
                await LoadDashboardDataAsync(teacherId.Value);
                return Page();
            }

            await _announcementService.CreateAnnouncementAsync(AnnouncementTitle, AnnouncementContent);

            Message = "Announcement created successfully!";
            ModelState.Clear();

            // Clear form
            AnnouncementTitle = string.Empty;
            AnnouncementContent = string.Empty;

            await LoadDashboardDataAsync(teacherId.Value);
            return Page();
        }

        private async Task LoadDashboardDataAsync(int teacherId)
        {
            TeacherId = teacherId;
            TeacherName = HttpContext.Session.GetString("AdminName") ?? "Teacher";

            RecentMessages = await _messageService.GetUserMessagesAsync(teacherId);
            RecentAnnouncements = await _announcementService.GetAllAnnouncementsAsync();

            var allSchedule = await _scheduleService.GetScheduleAsync();
            var today = DateTime.Now.DayOfWeek.ToString();

            var todayRows = allSchedule.AsEnumerable()
                .Where(row => row["DayOfWeek"].ToString() == today);

            if (todayRows.Any())
            {
                TodaySchedule = todayRows.CopyToDataTable();
            }
            else
            {
                TodaySchedule = allSchedule.Clone(); // Creates empty table with same schema
            }

            TodayClassesCount = TodaySchedule.Rows.Count;

            var assignments = await _assignmentService.GetAllAssignmentsAsync();
            PendingAssignmentsCount = assignments.Rows.Count;

            var subjects = await _subjectService.GetAllSubjectsAsync();
            SubjectCount = subjects.Rows.Count;
        }
    }
}