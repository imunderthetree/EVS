using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Parent
{
    public class ReportsModel : PageModel
    {
        private readonly TranscriptService _transcriptService;
        private readonly AttendanceService _attendanceService;

        public ReportsModel(TranscriptService transcriptService, AttendanceService attendanceService)
        {
            _transcriptService = transcriptService;
            _attendanceService = attendanceService;
        }

        public DataTable Reports { get; set; } = new DataTable();

        [BindProperty(SupportsGet = true)]
        public int StudentId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var parentId = HttpContext.Session.GetInt32("ParentId");
            if (!parentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            if (StudentId > 0)
            {
                Reports = await _transcriptService.GetStudentGradesAsync(StudentId);
            }

            return Page();
        }
    }
}