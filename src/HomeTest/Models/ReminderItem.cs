using System;
using HomeTest.Enums;

namespace HomeTest.Models
{
    public class ReminderItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public Priority Priority { get; set; }
        // Just to mark as it is important
        public bool Flagged { get; set; }
        public long RemindAt { get; set; }

        public ReminderItem(string name, string notes, Priority priority, bool flagged, long remindAt)
        {
            Id = Guid.NewGuid();
            Name = name;
            Notes = notes ?? "";
            Priority = priority == Priority.NotSet ? Priority.Low : priority;
            Flagged = flagged;
            RemindAt = remindAt;
        }
    }
}