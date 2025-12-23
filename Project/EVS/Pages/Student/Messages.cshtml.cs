using EVS.Data;
using EVS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EVS.Pages.Student
{
    public class MessagesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public MessagesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<MessageThread> Threads { get; set; }
        public List<Message> Messages { get; set; }
        [BindProperty]
        public string ReplyContent { get; set; }
        [BindProperty]
        public IFormFile Attachment { get; set; }
        [BindProperty]
        public int? SelectedThreadId { get; set; }

        public async Task OnGetAsync(int? threadId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Threads = await _context.MessageThreads
                .Include(t => t.Messages)
                .ThenInclude(m => m.Attachments)
                .Where(t => t.ParticipantIds.Contains(userId))
                .OrderByDescending(t => t.LastUpdated)
                .ToListAsync();

            if (threadId.HasValue)
            {
                SelectedThreadId = threadId;
                Messages = await _context.Messages
                    .Include(m => m.Attachments)
                    .Where(m => m.ThreadId == threadId)
                    .OrderByDescending(m => m.SentAt)
                    .ToListAsync();
                // Mark as read
                var unread = Messages.Where(m => m.RecipientId == userId && !m.IsRead).ToList();
                foreach (var msg in unread)
                {
                    msg.IsRead = true;
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IActionResult> OnPostReplyAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (SelectedThreadId == null || string.IsNullOrWhiteSpace(ReplyContent))
                return RedirectToPage();

            var thread = await _context.MessageThreads.FindAsync(SelectedThreadId);
            if (thread == null) return RedirectToPage();

            var recipientId = thread.ParticipantIds.FirstOrDefault(id => id != userId);
            var message = new Message
            {
                ThreadId = thread.Id,
                SenderId = userId,
                RecipientId = recipientId,
                Content = ReplyContent,
                SentAt = System.DateTime.UtcNow,
                IsRead = false
            };
            if (Attachment != null)
            {
                var filePath = $"uploads/{System.Guid.NewGuid()}_{Attachment.FileName}";
                using (var stream = System.IO.File.Create(filePath))
                {
                    await Attachment.CopyToAsync(stream);
                }
                message.Attachments.Add(new Attachment
                {
                    FileName = Attachment.FileName,
                    FilePath = filePath,
                    UploadedAt = System.DateTime.UtcNow
                });
            }
            _context.Messages.Add(message);
            thread.LastUpdated = System.DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return RedirectToPage(new { threadId = thread.Id });
        }
    }
}

