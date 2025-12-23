using System;

namespace EVS.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Teacher { get; set; }
    }

    public class TaskItem
    {
        public int Id { get; set; }
        public string Type { get; set; } // Assignment, Lesson, etc.
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public int CourseId { get; set; }
    }

    public class QuickLink
    {
        public string Label { get; set; }
        public string Url { get; set; }
    }
}
