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

        [HttpGet("Details")]
        public async Task<IActionResult> Details(string id)
        {
            var @event = await _mediator.Send(new EventByIdQuery(id));
            var calendar = await _mediator.Send(new CalendarByIdQuery(@event.CalendarId));
            if (@event is null) return View();
            return PartialView("Details", new EventDetailsViewModel()
            {
                Id = @event.Id,
                Title = @event.Title,
                Description = @event.Description,
                StartTime = @event.StartTime,
                EndTime = @event.EndTime,
                Calendar = calendar.Title,
                Location = @event.Location
            });
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            string userId = "1";
            var userCalendars = await _mediator.Send(new CalendarListQuery(userId));
            var selectedListItems = userCalendars.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Title
            }).ToList();
            var eventViewModel = new CreateEventViewModel
            {
                UserCalendars = selectedListItems,
                SelectedCalendarId = "0"
            };
            return View(eventViewModel);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventViewModel eventViewModel)
        {
/*            if (!ModelState.IsValid)
            {
                return View(eventViewModel);
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
        public async Task<IActionResult> Edit(string id)
        {
            string userId = "1";
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
                Id = id,
                Title = @event.Title,
                Description = @event.Description,
                StartTime = @event.StartTime,
                EndTime = @event.EndTime,
                Location = @event.Location,
                SelectedCalendarId = @event.CalendarId,
                UserCalendars = selectedListItems
            };
            return View(editEventViewModel);
        }

        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditEventViewModel eventViewModel)
        {
            /*            if (!ModelState.IsValid)
                        {
                            return View(eventViewModel); //user calendar is required
                        }*/
            var editedEvent = new Event()
            {
                Id = id,
                Description = eventViewModel.Description,
                Title = eventViewModel.Title,
                StartTime = eventViewModel.StartTime,
                EndTime = eventViewModel.EndTime,
                Location = eventViewModel.Location,
                CalendarId = eventViewModel.SelectedCalendarId
            };
            await _mediator.Send(new UpdateEventCommand(editedEvent));
            return RedirectToAction("Index", "Calendar");
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            _ = await _mediator.Send(new DeleteEventCommand(id));
            return RedirectToAction("Index", "Calendar");
        }
    }
}
