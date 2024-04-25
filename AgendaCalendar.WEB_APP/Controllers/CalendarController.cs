using AgendaCalendar.Application.Calendars.Commands;
using AgendaCalendar.Application.Calendars.Queries;
using AgendaCalendar.Application.Events.Queries;
using AgendaCalendar.Application.Reminders.Commands;
using AgendaCalendar.Application.Users.Queries.AgendaCalendar.Application.Users.Queries;
using AgendaCalendar.Domain.Entities;
using AgendaCalendar.WEB_APP.ViewModels.Calendar;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using _De_SerializationLib;

namespace AgendaCalendar.WEB_APP.Controllers
{
    [Route("[controller]")]
    public class CalendarController : Controller
    {
        private readonly IMediator _mediator;
        public CalendarController(IMediator mediator) 
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = "e4019057-c887-4add-9961-a2651e328b85";
            var calendarId = "1";
            var userCalendars = await _mediator.Send(new CalendarListQuery(userId));
            List<Event> events = new();
            foreach(var calendar in userCalendars)
            {
                events = events.Concat(calendar.Events).ToList();
            }
            var upcomingEvents = await _mediator.Send(new EventListByDateQuery(calendarId, DateTime.Now));

            if(events.Any())
            {
                ViewData["Events"] = JsonConverter.GetJsonEventList(events);
            }
            if(userCalendars.Any())
            {
                ViewData["Calendars"] = userCalendars;
            }
            if(upcomingEvents.Any())
            {
                ViewData["UpcomingEvents"] = upcomingEvents.Take(3);
            }
            return View();
        }

        [HttpGet("Export")]
        public async Task<IActionResult> Export(string id) 
        {
            var bytes = await _mediator.Send(new ExportCalendarCommand(id));
            var calendar = await _mediator.Send(new CalendarByIdQuery(id));
            return File(bytes, "text/plain", $"{calendar.Title}.ics");
        }

        [HttpPost("Import")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile file)
        {
            string user_id = "1";
            string filename = Path.GetFileName(file.FileName);
            using MemoryStream mstream = new();
            await file.CopyToAsync(mstream);
            byte[] calendar_bytes = mstream.ToArray();
            var calendar = await _mediator.Send(new ImportCalendarCommand(calendar_bytes, user_id));
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            //var curUserId = HttpContext.User.GetUserId();
            var createCalendarViewModel = new CreateCalendarViewModel();
            return View(createCalendarViewModel);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCalendarViewModel calendarViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(calendarViewModel);
            }

            var user = await _mediator.Send(new UserByEmailQuery(calendarViewModel.OwnerEmail));
            if (user is not null)
            {
                var calendar = await _mediator.Send(new CreateCalendarCommand(calendarViewModel.Title, calendarViewModel.Description, user.Id));
                if (calendar == null) return View(calendarViewModel);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            var calendar = await _mediator.Send(new CalendarByIdQuery(id));
            if (calendar == null) return View();
            return View(new EditCalendarViewModel()
            {
                Description = calendar.CalendarDescription,
                Title = calendar.Title,
                Id = calendar.Id
            });
        }

        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditCalendarViewModel calendarViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(calendarViewModel);
            }
            var new_calendar = await _mediator.Send(new UpdateCalendarCommand(id, calendarViewModel.Description, calendarViewModel.Title));
            if(new_calendar is  null)
            {
                return View(calendarViewModel);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var calendar = await _mediator.Send(new CalendarByIdQuery(id));
            if (calendar == null) return View();
            calendar = await _mediator.Send(new DeleteCalendarCommand(id));
            if (calendar is null) return View();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Subscribe")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe(string id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("Unsubscribe")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unsubscribe(string id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("Reminders/Create")]
        public IActionResult CreateReminder()
        {
            throw new NotImplementedException();
        }

        [HttpPost("Reminders/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReminder(string id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("Reminders/Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReminder(string id)
        {
            var reminder = await _mediator.Send(new DeleteReminderCommand(id));
            if (reminder == null) return View("Error");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Reminders/Edit")]
        public IActionResult EditReminder(string id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("Reminders/Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReminder(string id, IFormCollection form)
        {
            throw new NotImplementedException();
        }

    }
}
