using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

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
}