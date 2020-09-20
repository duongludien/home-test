using System;
using HomeTest.Enums;

namespace HomeTest.Dto
{
    public class ReminderItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public Priority Priority { get; set; }
        public bool Flagged { get; set; }
        public long RemindAt { get; set; }
    }
}