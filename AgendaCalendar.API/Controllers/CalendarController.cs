using _De_SerializationLib;
using AgendaCalendar.API.ViewModels.Calendar;
using AgendaCalendar.Application.Calendars.Commands;
using AgendaCalendar.Application.Calendars.Queries;
using AgendaCalendar.Application.Events.Queries;
using AgendaCalendar.Application.Reminders.Commands;
using AgendaCalendar.Application.Users.Queries.AgendaCalendar.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AgendaCalendar.API.Controllers
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
            int userId = 1;
            int calendarId = 1;
            var events = await _mediator.Send(new EventListQuery(calendarId));
            var userCalendars = await _mediator.Send(new CalendarListQuery(userId));
            var upcomingEvents = await _mediator.Send(new EventListByDateQuery(calendarId, DateTime.Now));

            ViewData["Events"] = JsonConverter.GetJsonEventList(events);
            ViewData["Calendars"] = userCalendars;
            ViewData["UpcomingEvents"] = upcomingEvents.Take(3);
            return View();
        }

        [HttpGet("Export")]
        public async Task<IActionResult> Export(int id) 
        {
            var bytes = await _mediator.Send(new ExportCalendarCommand(id));
            var calendar = await _mediator.Send(new CalendarByIdQuery(id));
            return File(bytes, "text/plain", $"{calendar.Title}.ics");
        }

        [HttpPost("Import")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile file)
        {
            int user_id = 1;
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
        public async Task<IActionResult> Edit(int id)
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
        public async Task<IActionResult> Edit(EditCalendarViewModel calendarViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(calendarViewModel);
            }
            var new_calendar = await _mediator.Send(new UpdateCalendarCommand(calendarViewModel.Id, calendarViewModel.Description, calendarViewModel.Title));
            if(new_calendar is  null)
            {
                return View();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var calendar = await _mediator.Send(new CalendarByIdQuery(id));
            if (calendar == null) return View();
            calendar = await _mediator.Send(new DeleteCalendarCommand(id));
            if (calendar is null) return View();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Subscribe")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("Unsubscribe")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unsubscribe(int id)
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
        public async Task<IActionResult> CreateReminder(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("Reminders/Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReminder(int id)
        {
            var reminder = await _mediator.Send(new DeleteReminderCommand(id));
            if (reminder == null) return View("Error");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Reminders/Edit")]
        public IActionResult EditReminder(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("Reminders/Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReminder(int id, IFormCollection form)
        {
            throw new NotImplementedException();
        }

    }
}
