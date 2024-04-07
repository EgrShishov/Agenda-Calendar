using AgendaCalendar.API.ViewModels.Event;
using AgendaCalendar.Application.Calendars.Queries;
using AgendaCalendar.Application.Events.Commands;
using AgendaCalendar.Application.Events.Queries;
using AgendaCalendar.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AgendaCalendar.API.Controllers
{
    [Route("[controller]")]
    public class EventController : Controller
    {
        private readonly IMediator _mediator;
        public EventController(IMediator mediator) 
        {
            _mediator = mediator;
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            int userId = 1;
            var userCalendars = await _mediator.Send(new CalendarListQuery(userId));
            var selectedListItems = userCalendars.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Title
            }).ToList();
            var eventViewModel = new CreateEventViewModel
            {
                UserCalendars = selectedListItems,
                SelectedCalendarId = 0
            };
            return View(eventViewModel);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventViewModel eventViewModel)
        {
/*            if (!ModelState.IsValid)
            {
                return View(eventViewModel); //user calendar field is required
            }*/
            var @event = new Event()
            {
                Title = eventViewModel.Title,
                Description = eventViewModel.Description,
                StartTime = eventViewModel.StartTime,
                EndTime = eventViewModel.EndTime,
                Location = eventViewModel.Location
            };
            var response = await _mediator.Send(new AddEventCommand(eventViewModel.SelectedCalendarId, @event));
            if (response is null) return View(eventViewModel);
            return RedirectToAction("Index", "Calendar");
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            int userId = 1;
            var userCalendars = await _mediator.Send(new CalendarListQuery(userId));
            var selectedListItems = userCalendars.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Title
            }).ToList();
            var @event = await _mediator.Send(new EventByIdQuery(id));
            if (@event == null) return View();
            var editEventViewModel = new EditEventViewModel()
            {
                Title = @event.Title,
                Description = @event.Description,
                StartTime = @event.StartTime,
                EndTime = @event.EndTime,
                Location = @event.Location,
                SelectedCalendarId = 0,
                UserCalendars = selectedListItems
            };
            return View(editEventViewModel);
        }

        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditEventViewModel eventViewModel)
        {
/*            if (!ModelState.IsValid)
            {
                return View(eventViewModel); //user calendar is required
            }*/
            var editedEvent = new Event()
            {
                Id = eventViewModel.Id,
                Description = eventViewModel.Description,
                Title = eventViewModel.Title,
                StartTime = eventViewModel.StartTime,
                EndTime = eventViewModel.EndTime,
                Location = eventViewModel.Location,
            };
            var updatedEvent = await _mediator.Send(new UpdateEventCommand(editedEvent));
            return RedirectToAction("Index", "Calendar");
        }

        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            _ = await _mediator.Send(new DeleteEventCommand(id));
            return RedirectToAction("Index", "Calendar");
        }
    }
}
