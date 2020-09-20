using System;
using System.Collections.Generic;
using HomeTest.Dto;
using HomeTest.Models;

namespace HomeTest.Services
{
    public interface IReminderItemService
    { 
        ReminderItem GetById(Guid id);
        IEnumerable<ReminderItem> GetAll();
        IEnumerable<ReminderItem> Get(GetReminderItemsFilters filters);
        Guid Create(ReminderItemDto input);
        void Update(ReminderItemDto input);
        void Delete(Guid id);
    }
}