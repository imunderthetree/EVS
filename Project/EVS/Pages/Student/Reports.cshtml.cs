using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EVS.Data;
using EVS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text;

namespace EVS.Pages.Student
{
    public class ReportsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ReportsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Placeholder for reports (to be replaced with DB logic)
        public List<string> Reports { get; set; }

        public List<Message> Messages { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Messages = await _context.Messages
                .Where(m => m.SenderId == userId || m.RecipientId == userId)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();

            // TODO: Replace with database fetching logic
            Reports = new List<string> { "Report Card - Term 1", "Report Card - Term 2" };

            // TODO: Generate downloadable report (CSV/PDF) if needed
            return Page();
        }

        public async Task<IActionResult> OnGetCsvAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var messages = await _context.Messages
                .Where(m => m.SenderId == userId || m.RecipientId == userId)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();
            var sb = new StringBuilder();
            sb.AppendLine("SentAt,SenderId,RecipientId,Content,IsRead");
            foreach (var m in messages)
            {
                sb.AppendLine($"{m.SentAt},{m.SenderId},{m.RecipientId},\"{m.Content.Replace("\"", "''")}\",{m.IsRead}");
            }
            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "messages_report.csv");
        }

        public async Task<IActionResult> OnGetPdfAsync()
        {
            // For demonstration, generate a simple PDF using QuestPDF or similar (pseudo-code)
            // In real use, add QuestPDF or another library and implement PDF generation
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var messages = await _context.Messages
                .Where(m => m.SenderId == userId || m.RecipientId == userId)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();
            // TODO: Replace with real PDF generation
            var pdfBytes = Encoding.UTF8.GetBytes("PDF generation not implemented.\nMessages count: " + messages.Count);
            return File(pdfBytes, "application/pdf", "messages_report.pdf");
        }
    }
}