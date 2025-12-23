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
        private readonly TeacherService _teacherService;

        public MessagesModel(MessageService messageService, TeacherService teacherService)
        {
            _messageService = messageService;
            _teacherService = teacherService;
        }

        public DataTable Messages { get; set; } = new DataTable();

        public int StudentId { get; set; }

        // Dictionary to map user IDs to names
        public Dictionary<int, string> UserNames { get; set; } = new();

        [BindProperty]
        public int ReceiverId { get; set; }

        [BindProperty]
        public string MessageContent { get; set; } = string.Empty;

        [BindProperty]
        public IFormFile? Attachment { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SelectedThreadId { get; set; }

        public List<ThreadViewModel> Threads { get; set; } = new List<ThreadViewModel>();

        [BindProperty]
        public string ReplyContent { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (!studentId.HasValue)
            {
                return RedirectToPage("/Account/Login");
            }

            StudentId = studentId.Value;
            Messages = await _messageService.GetUserMessagesAsync(studentId.Value);

            // Load teacher names for display
            await LoadUserNamesAsync();

            // Build threads from messages grouped by the other participant
            Threads = BuildThreadsFromMessages(Messages, studentId.Value);

            // Auto-select first thread if none selected
            if (string.IsNullOrEmpty(SelectedThreadId) && Threads.Count > 0)
            {
                SelectedThreadId = Threads[0].Id;
            }

            return Page();
        }

        private async Task LoadUserNamesAsync()
        {
            // Load all teachers
            var teachers = await _teacherService.GetAllTeachersAsync();
            foreach (DataRow row in teachers.Rows)
            {
                var id = Convert.ToInt32(row["TeacherID"]);
                var name = row["FullName"]?.ToString() ?? $"Teacher #{id}";
                UserNames[id] = name;
            }
        }

        public string GetUserName(int userId)
        {
            return UserNames.TryGetValue(userId, out var name) ? name : $"User #{userId}";
        }

        private List<ThreadViewModel> BuildThreadsFromMessages(DataTable messages, int currentUserId)
        {
            var threadDict = new Dictionary<int, ThreadViewModel>();

            foreach (DataRow row in messages.Rows)
            {
                var senderId = Convert.ToInt32(row["SenderID"]);
                var receiverId = Convert.ToInt32(row["ReceiverID"]);
                var otherUserId = senderId == currentUserId ? receiverId : senderId;

                if (!threadDict.ContainsKey(otherUserId))
                {
                    threadDict[otherUserId] = new ThreadViewModel
                    {
                        Id = otherUserId.ToString(),
                        ParticipantIds = new List<string> { currentUserId.ToString(), otherUserId.ToString() },
                        Messages = new List<MessageViewModel>(),
                        LastUpdated = DateTime.MinValue
                    };
                }

                var sentDate = Convert.ToDateTime(row["SentDate"]);
                threadDict[otherUserId].Messages.Add(new MessageViewModel
                {
                    SenderId = senderId.ToString(),
                    Content = row["MessageContent"]?.ToString() ?? string.Empty,
                    SentAt = sentDate
                });

                if (sentDate > threadDict[otherUserId].LastUpdated)
                {
                    threadDict[otherUserId].LastUpdated = sentDate;
                }
            }

            return threadDict.Values.OrderByDescending(t => t.LastUpdated).ToList();
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

            return RedirectToPage(new { SelectedThreadId = ReceiverId.ToString() });
        }
    }

    public class ThreadViewModel
    {
        public string Id { get; set; } = string.Empty;
        public List<string> ParticipantIds { get; set; } = new List<string>();
        public List<MessageViewModel> Messages { get; set; } = new List<MessageViewModel>();
        public DateTime LastUpdated { get; set; }
    }

    public class MessageViewModel
    {
        public string SenderId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
    }
}