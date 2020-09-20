using System;
using HomeTest.Dto;
using HomeTest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HomeTest.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ReminderItemsController : ControllerBase
    {
        private readonly ILogger<ReminderItemsController> _logger;
        private readonly IReminderItemService _reminderItemService;

        public ReminderItemsController(ILogger<ReminderItemsController> logger, 
            IReminderItemService reminderItemService)
        {
            _logger = logger;
            _reminderItemService = reminderItemService;
        }

        [HttpGet("GetById")]
        public IActionResult GetById(Guid? id)
        {
            if (id == null) return BadRequest();

            var reminderItem = _reminderItemService.GetById(id.Value);
            if (reminderItem != null) return Ok(reminderItem);
            return NotFound();
        }

        [HttpGet("Get")]
        public IActionResult Get(GetReminderItemsFilters? filters)
        {
            var items = _reminderItemService.Get(filters);
            return Ok(items);
        }

        [HttpPost("Create")]
        public IActionResult Create(ReminderItemDto input)
        {
            if (string.IsNullOrEmpty(input.Name))
            {
                return BadRequest();
            }

            var id = _reminderItemService.Create(input);
            return Ok(id);
        }

        [HttpPost("Update")]
        public IActionResult Update(ReminderItemDto input)
        {
            var oldReminderItem = _reminderItemService.GetById(input.Id);
            if (oldReminderItem == null) return NotFound();
            
            _reminderItemService.Update(input);
            return Ok(input.Id);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null) return BadRequest();

            var reminderItem = _reminderItemService.GetById(id.Value);
            if (reminderItem == null) return NotFound();
            
            _reminderItemService.Delete(id.Value);
            return Ok(id);
        }
    }
}