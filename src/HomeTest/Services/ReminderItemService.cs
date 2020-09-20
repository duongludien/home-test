using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HomeTest.Dto;
using HomeTest.Enums;
using HomeTest.Models;
using Microsoft.Extensions.Caching.Memory;

namespace HomeTest.Services
{
    public class ReminderItemService : IReminderItemService
    {
        private readonly IMemoryCache _cache;

        public ReminderItemService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public ReminderItem GetById(Guid id)
        {
            var reminderItem = _cache.Get<ReminderItem>(id);
            return reminderItem;
        }
        
        private IEnumerable<Guid> GetAllKeysList()
        {
            var field = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            
            var items = new List<Guid>();
            if (field == null) return items;

            if (!(field.GetValue(_cache) is ICollection collection)) return items;
            
            foreach (var item in collection)
            {
                var methodInfo = item.GetType().GetProperty("Key");
                var val = methodInfo?.GetValue(item);
                items.Add(Guid.Parse(val.ToString()));
            }
            return items;
        }

        public IEnumerable<ReminderItem> GetAll()
        {
            var keys = GetAllKeysList();
            var items = keys.Select(key => _cache.Get<ReminderItem>(key)).ToList();
            return items;
        }

        public IEnumerable<ReminderItem> Get(GetReminderItemsFilters filters)
        {
            var items = GetAll();
            
            if (filters == null) return items;
            
            // Filters contains Id, there is just one item or not 
            if (!filters.Id.Equals(Guid.Empty))
            {
                items = items.Where(r => r.Id == filters.Id).ToList();
                return items;
            }
            
            // Filter by name
            if (!string.IsNullOrEmpty(filters.Name))
            {
                items = items
                    .Where(r => r.Name.IndexOf(filters.Name, StringComparison.CurrentCultureIgnoreCase) != -1)
                    .ToList();
            }
            
            // Filter by priority
            if (filters.Priority != Priority.NotSet)
            {
                items = items
                    .Where(r => r.Priority == filters.Priority)
                    .ToList();
            }
            
            // Filter by flag only
            if (filters.FlaggedOnly)
            {
                items = items
                    .Where(r => r.Flagged)
                    .ToList();
            }
            
            // Filter by time
            if (filters.RemindAtStart != 0 && filters.RemindAtEnd != 0)
            {
                items = items
                    .Where(r => r.RemindAt >= filters.RemindAtStart && r.RemindAt <= filters.RemindAtEnd)
                    .ToList();
            }

            return items;
        }

        public Guid Create(ReminderItemDto input)
        {
            var reminderItem = new ReminderItem(input.Name, input.Notes, input.Priority, input.Flagged, input.RemindAt);
            _cache.Set(reminderItem.Id, reminderItem);
            return reminderItem.Id;
        }

        public void Update(ReminderItemDto input)
        {
            var reminderItem = GetById(input.Id);

            if (!string.IsNullOrEmpty(input.Name)) reminderItem.Name = input.Name;
            if (!string.IsNullOrEmpty(input.Notes)) reminderItem.Notes = input.Notes;
            if (input.Priority != Priority.NotSet) reminderItem.Priority = input.Priority;
            reminderItem.Flagged = input.Flagged;
            reminderItem.RemindAt = input.RemindAt;
            
            _cache.Remove(input.Id);
            _cache.Set(input.Id, reminderItem);
        }

        public void Delete(Guid id)
        {
            _cache.Remove(id);
        }
    }
}