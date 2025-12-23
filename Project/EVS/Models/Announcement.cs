using System;

namespace EVS.Models
{
    public class Announcement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Course { get; set; }
        public DateTime PostedAt { get; set; }
    }
}
