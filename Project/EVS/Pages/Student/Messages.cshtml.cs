using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;
using System.Collections.Generic;

namespace EVS.Pages.Student
{
    public class MessagesModel : PageModel
    {
        private readonly MessageService _messageService;

        public MessagesModel(MessageService messageService)
        {
            _messageService = messageService;
        }

        public DataTable Messages { get; set; } = new DataTable();

        [BindProperty]
        public int ReceiverId { get; set; }

        [BindProperty]
        public string MessageContent { get; set; } = string.Empty;

        [BindProperty]
        public IFormFile Attachment { get; set; }

        public string? SelectedThreadId { get; set; }

        // Add this property to expose Threads to the Razor page
        public List<ThreadViewModel> Threads { get; set; }

        [BindProperty]
        public string ReplyContent { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (!studentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Messages = await _messageService.GetUserMessagesAsync(studentId.Value);

            return Page();
        }

        public async Task<IActionResult> OnPostSendAsync()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (!studentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            if (string.IsNullOrWhiteSpace(MessageContent))
            {
                return Page();
            }

            await _messageService.SendMessageAsync(studentId.Value, ReceiverId, MessageContent);

            return RedirectToPage();
        }
    }

    // Example ThreadViewModel definition (adjust as per your actual model)
    public class ThreadViewModel
    {
        public string Id { get; set; }
        public List<string> ParticipantIds { get; set; }
        public List<MessageViewModel> Messages { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class MessageViewModel
    {
        public string SenderId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}