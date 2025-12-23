using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using EVS.Services;

namespace EVS.Pages.Parent
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
            var parentId = HttpContext.Session.GetInt32("ParentId");
            if (!parentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            Messages = await _messageService.GetUserMessagesAsync(parentId.Value);

            return Page();
        }

        public async Task<IActionResult> OnPostSendAsync()
        {
            var parentId = HttpContext.Session.GetInt32("ParentId");
            if (!parentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            if (string.IsNullOrWhiteSpace(MessageContent))
            {
                return Page();
            }

            await _messageService.SendMessageAsync(parentId.Value, ReceiverId, MessageContent);

            return RedirectToPage();
        }
    }
}
