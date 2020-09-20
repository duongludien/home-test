using System;
using HomeTest.Enums;

namespace HomeTest.Dto
{
    public class GetReminderItemsFilters
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Priority Priority { get; set; } = Priority.NotSet;
        public bool FlaggedOnly { get; set; }
        public long RemindAtStart { get; set; }
        public long RemindAtEnd { get; set; }
    }
}