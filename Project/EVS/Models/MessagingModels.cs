using System;
using System.Collections.Generic;

namespace EVS.Models
{
    public class MessageThread
    {
        public int Id { get; set; }
        public List<Message> Messages { get; set; } = new();
        public List<string> ParticipantIds { get; set; } = new();
        public DateTime LastUpdated { get; set; }
    }

    public class Message
    {
        public int Id { get; set; }
        public int ThreadId { get; set; }
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
        public List<Attachment> Attachments { get; set; } = new();
    }

    public class Attachment
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
    }

    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
