using AgendaCalendar.Application;
using AgendaCalendar.Application.Calendars.Commands;
using AgendaCalendar.Application.Calendars.Queries;
using AgendaCalendar.Application.Events.Commands;
using AgendaCalendar.Application.Events.Queries;
using AgendaCalendar.Application.Reminders.Commands;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.DependencyInjection;
using _De_SerializationLib;
using Microsoft.Extensions.Configuration;

class Program {

    private static IMediator? mediator;
    static void Main(string[] args)
    {
        while (true) { }
        var configuration = new ConfigurationManager();
        //DI
        var services = new ServiceCollection()
            .AddApplication()
            //.AddInfrastructure(configuration)
            .BuildServiceProvider();

        //Mediatr
        mediator = services.GetRequiredService<IMediator>();
        GlobalConfiguration.Configuration.UsePostgreSqlStorage("User ID=postgres;Password=rootroot;Host=localhost;Port=5432;Database=hangfire;");
        //var mama = Task.Run(async () => await mediator.Send(new AuthenticationThroughGoogleQuery())).Result;

        var server = new BackgroundJobServer();
        var dashboard = new BackgroundJobServer();
        RecurringJob.AddOrUpdate("my-recurring-job", () => CheckAndSendNotifications(), Cron.MinuteInterval(1));
        RecurringJob.AddOrUpdate("birthday-checking", () => CheckForUsersBirthday(), Cron.Daily);
        Console.WriteLine("Hangfire server started.");
/*        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "WD4hKlHHq6goYZ2bADFAtmiKi8CscnClInv6Tkpm",
            BasePath = "https://agendacalendar-418020-default-rtdb.europe-west1.firebasedatabase.app/"
        };
        IFirebaseClient client = new FirebaseClient(config);
        var response = Task.Run(async () => await client.PushAsync(Path.Combine(config.BasePath, "/users"),
            new User() { UserName = "EgorShishov", Password = "Mama", Email = "lolka@gmail.com", BirthdayDate= Convert.ToDateTime("01.12.2004") }));
        Console.WriteLine(response.Result);*/
        /*        var response2 = Task.Run(async () => await client.GetAsync(Path.Combine(config.BasePath, $"/users/-NtrjbnR07Y8MEXS5HYu")));
                Console.WriteLine(response2.Result.Body);*/
        Console.WriteLine("pidoras");
        User user = null;
        var eve = new Event() { Title = "FEED A CAT!!!", StartTime = DateTime.Now.AddDays(-2), EndTime = DateTime.Now, Description = "Pisya", AuthorId = 0 };
        //var @event = Task.Run(async () => await mediator.Send(new AddEventCommand(2037510250, eve))).Result;
        var reminder = Task.Run(async () => await mediator.Send(new AddReminderCommand("Feed a cat", DateTime.Now, "rosto4eks@gmail.com", 0))).Result;
        reminder = Task.Run(async () => await mediator.Send(new AddReminderCommand("Feed a dog", DateTime.Now, "e.shishov98@yandex.ru", 0))).Result;

       /* var recurring_event = Task.Run(async () => await mediator.Send(new AddEventCommand(1020436601, new RecurringEvent()
        {
            AuthorId = 566795566,
            ReccurenceRules = new RecurrenceRule
            {
                Frequency = RecurrenceFrequency.Monthly,
                Interval = 1,
                DaysOfMonth = new List<int>() { 15 }, // Repeat on the 15th day of each month
                RecurrenceDates = new List<DateTime> { new DateTime(2024, 3, 20), new DateTime(2024, 4, 20) } // Optional list of specific dates
            },
            Title = "First recurring event",
            Description = "Medical treatment",
            StartTime = DateTime.Now,
        }))).Result;*/

        while (user == null)
        {
            Console.WriteLine(user);
            user = Task.Run(async () => await AuthorisationMenu(mediator)).Result;
        }

        int choice = 0;

        var calendar = Task.Run(async () => await mediator.Send(new CalendarByIdQuery(2037510250))).Result;

        do
        {
            Console.WriteLine("Main Menu:");
            Console.WriteLine("1. Calendars");
            Console.WriteLine("2. Reminders");
            Console.WriteLine("4. Exit");

            Console.WriteLine(calendar.CalendarDescription);
            var str = IcalConverter.Serialize(calendar);
            Console.WriteLine(str);
            //var deserialized = IcalConverter.Deserialize("BEGIN:VCALENDAR\r\nVERSION:2.0\r\nCALSCALE:GREGORIAN\r\nBEGIN:VEVENT\r\nSUMMARY:Access-A-Ride Pickup\r\nDTSTART;TZID=America/New_York:20130802T103400\r\nDTEND;TZID=America/New_York:20130802T110400\r\nLOCATION:1000 Broadway Ave.\\, Brooklyn\r\nDESCRIPTION: Access-A-Ride to 900 Jay St.\\, Brooklyn\r\nSTATUS:CONFIRMED\r\nSEQUENCE:3\r\nBEGIN:VALARM\r\nTRIGGER:-PT10M\r\nDESCRIPTION:Pickup Reminder\r\nACTION:DISPLAY\r\nEND:VALARM\r\nEND:VEVENT\r\nBEGIN:VEVENT\r\nSUMMARY:Access-A-Ride Pickup\r\nDTSTART;TZID=America/New_York:20130802T200000\r\nDTEND;TZID=America/New_York:20130802T203000\r\nLOCATION:900 Jay St.\\, Brooklyn\r\nDESCRIPTION: Access-A-Ride to 1000 Broadway Ave.\\, Brooklyn\r\nSTATUS:CONFIRMED\r\nSEQUENCE:3\r\nBEGIN:VALARM\r\nTRIGGER:-PT10M\r\nDESCRIPTION:Pickup Reminder\r\nACTION:DISPLAY\r\nEND:VALARM\r\nEND:VEVENT\r\nEND:VCALENDAR");
            //Console.WriteLine(deserialized.CalendarDescription, deserialized.Title);

            Console.Write("Enter your choice: ");
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                Console.Clear();
                switch (choice)
                {
                    case 1:
                        CalendarsMenu(user, mediator);
                        break;
                    case 2:
                        RemindersMenu(user, mediator);
                        break;
                    case 4:
                        Console.WriteLine("Exiting application. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        } while (choice != 4);

        while (true)
        {
            //emulates server
        }
        
    }
    //checks for reminders
    public static void CheckAndSendNotifications()
    {
        mediator.Send(new CheckRemindersCommand()).Wait();
        //Console.WriteLine("Sended");
    }
    //GUI
    public static void CheckForUsersBirthday()
    {
        mediator.Send(new SendBirthdayRemindersCommand()).Wait();
        Console.WriteLine("Birthday's checked");
    }
    //done
    static async Task<User> AuthorisationMenu(IMediator mediator)
    {
        while (true)
        {
            Console.Clear();
            var choice = -1;
            Console.WriteLine("Login Menu:");
            Console.WriteLine("1. Auth");
            Console.WriteLine("2. Registr");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice: ");
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    Console.WriteLine("Authenticating...");
                    Console.WriteLine("Enter username");
                    var username = Console.ReadLine();
                    Console.WriteLine("Enter password");
                    var password = Console.ReadLine();

                    var user = await mediator.Send(new LoginQuery(username, password));
                    if(user == null)
                    {
                        Console.WriteLine("Access denied");
                        await Task.Delay(10);
                    }
                    return user;
                case 2:
                    Console.WriteLine("Registartion");
                    Console.WriteLine("Enter username");
                    username = Console.ReadLine();
                    Console.WriteLine("Enter password");
                    password = Console.ReadLine();
                    Console.WriteLine("Enter email");
                    var email = Console.ReadLine();

                    var new_user = await mediator.Send(new RegisterCommand(username, password, email));
                    return new_user;
                case 3:
                    Console.WriteLine("Exiting application. Goodbye!");
                    return null;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    continue;
            }
        }
    }
    //done, except editing events with reccuring types currently unavaibles
    static async void CalendarEventsMenu(User user, Calendar calendar, IMediator mediator)
    {
        while (true)
        {
            Console.WriteLine("Events Menu:");
            Console.WriteLine("1. View Events");
            Console.WriteLine("2. Add Event");
            Console.WriteLine("3. Add Reccuring Event");
            Console.WriteLine("4. Edit Event");
            Console.WriteLine("5. Delete Event");
            Console.WriteLine("6. Add participant for event");
            Console.WriteLine("7. Show ewents for today");
            Console.WriteLine("8. Show events for the week");
            Console.WriteLine("9. Show events for the month");
            Console.WriteLine("10. Show events for the year");
            Console.WriteLine("11. Show events for the date");
            Console.WriteLine("12. Show events for the date range");
            Console.WriteLine("13. Back to Main Menu");

            var choice = -1;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    foreach (var eve in calendar.Events)
                    {
                        Console.Write(eve.Title, ", ");
                    }
                    break;
                case 2:
                    Console.WriteLine("Event name:");
                    var eventName = Console.ReadLine();
                    Console.WriteLine("Event description:");
                    var description = Console.ReadLine();
                    Console.WriteLine("Enter start time:");
                    DateTime.TryParse(Console.ReadLine(), out DateTime start);
                    Console.WriteLine("Enter end time");
                    DateTime.TryParse(Console.ReadLine(), out DateTime end);
                    var @event = await mediator.Send(new AddEventCommand(calendar.Id, new Event() { Title = eventName, StartTime = start, EndTime = end, Description = description, AuthorId = user.Id }));
                    Console.WriteLine("Event added!");
                    break;
                case 3:
                    Console.WriteLine("Event name:");
                    eventName = Console.ReadLine();
                    Console.WriteLine("Event description:");
                    description = Console.ReadLine();
                    Console.WriteLine("Enter start time:");
                    DateTime.TryParse(Console.ReadLine(), out start);
                    Console.WriteLine("Enter end time");
                    DateTime.TryParse(Console.ReadLine(), out end);
                    Console.WriteLine("Enter interval");
                    int.TryParse(Console.ReadLine(), out int interval);
                    Console.WriteLine("Enter reccurency type: 1 - Daily, 2 - Weekly, 3 - Monthly, 4 - Yearly");
                    int.TryParse(Console.ReadLine(), out int type);
                    RecurrenceFrequency rec_type = RecurrenceFrequency.Daily;
                    switch (type)
                    {
                        case 1:
                            rec_type = RecurrenceFrequency.Daily;
                            break;
                        case 2:
                            rec_type = RecurrenceFrequency.Weekly;
                            break;
                        case 3:
                            rec_type = RecurrenceFrequency.Monthly;
                            break;
                        case 4:
                            rec_type = RecurrenceFrequency.Yearly;
                            break;
                    }
                    break;
                case 4:
                    Console.WriteLine("Enter event title to edit:");
                    var event_name = Console.ReadLine();
                    foreach (var eve in calendar.Events)
                    {
                        if (eve.Title == event_name)
                        {
                            Console.WriteLine("New event name:");
                            var neweventName = Console.ReadLine();
                            Console.WriteLine("New event description:");
                            var newdescription = Console.ReadLine();
                            Console.WriteLine("Enter start time:");
                            DateTime.TryParse(Console.ReadLine(), out DateTime newstart);
                            Console.WriteLine("Enter end time");
                            DateTime.TryParse(Console.ReadLine(), out DateTime newend);

                            eve.Title = neweventName;
                            eve.Description = newdescription;
                            eve.StartTime = newstart;
                            eve.EndTime = newend;
                            await mediator.Send(new UpdateEventCommand(eve));
                            Console.WriteLine("Event updated");
                            break;
                        }
                    }
                    Console.WriteLine("Cannot find event");
                    break;
                case 5:
                    Console.WriteLine("Enter event name to delete");
                    event_name = Console.ReadLine();
                    foreach (var eve in calendar.Events)
                    {
                        if(eve.Title == event_name)
                        {
                            await mediator.Send(new DeleteEventCommand(calendar.Id, eve));
                            Console.WriteLine("Event deleted");
                            break;
                        }
                    }
                    Console.WriteLine("Cannot find event");
                    break;
                case 6: //adding participant for event
                    //penis = v_popu

                    break;
                case 7://for the day
                    var day_events = await mediator.Send(new EventListByDateQuery(calendar.Id, DateTime.Now.Date));
                    if (day_events != null)
                    {
                        foreach(var day_event in day_events)
                        {
                            Console.WriteLine(day_event.Title + ", " + day_event.Description + ", " + day_event.EndTime);
                        }
                    }
                    else Console.WriteLine("There are no events for today");
                    break;
                case 8:
                    var week_events = await mediator.Send(new EventListByDateRangeQuery(calendar.Id, DateTime.Now.Date, DateTime.Now.AddDays(7)));
                    if (week_events != null)
                    {
                        foreach (var day_event in week_events)
                        {
                            Console.WriteLine(day_event.Title + ", " + day_event.Description + ", " + day_event.EndTime);
                        }
                    }
                    else Console.WriteLine("There are no events for that week");
                    break;
                case 9:
                    var month_events = await mediator.Send(new EventListByDateRangeQuery(calendar.Id, DateTime.Now.Date, DateTime.Now.AddMonths(1)));
                    if (month_events != null)
                    {
                        foreach (var day_event in month_events)
                        {
                            Console.WriteLine(day_event.Title + ", " + day_event.Description + ", " + day_event.EndTime);
                        }
                    }
                    else Console.WriteLine("There are no events for that month");
                    break;
                case 10:
                    var year_events = await mediator.Send(new EventListByDateRangeQuery(calendar.Id, DateTime.Now.Date, DateTime.Now.AddYears(1)));
                    if (year_events != null)
                    {
                        foreach (var day_event in year_events)
                        {
                            Console.WriteLine(day_event.Title + ", " + day_event.Description + ", " + day_event.EndTime);
                        }
                    }
                    else Console.WriteLine("There are no events for that year");
                    break;
                case 11:
                    Console.WriteLine("Enter date: ");
                    DateTime.TryParse(Console.ReadLine(), out DateTime entered_date);
                    var events = await mediator.Send(new EventListByDateQuery(calendar.Id, entered_date));
                    if (events != null)
                    {
                        foreach (var day_event in events)
                        {
                            Console.WriteLine(day_event.Title + ", " + day_event.Description + ", " + day_event.EndTime);
                        }
                    }
                    else Console.WriteLine("There are no events for that day");
                    break;
                case 12:
                    Console.WriteLine("Enter start date: ");
                    DateTime.TryParse(Console.ReadLine(), out entered_date);
                    Console.WriteLine("Enter end date: ");
                    DateTime.TryParse(Console.ReadLine(), out DateTime end_entered_date);
                    var range_events = await mediator.Send(new EventListByDateRangeQuery(calendar.Id, entered_date, end_entered_date));
                    if (range_events != null)
                    {
                        foreach (var day_event in range_events)
                        {
                            Console.WriteLine(day_event.Title + ", " + day_event.Description + ", " + day_event.EndTime);
                        }
                    }
                    else Console.WriteLine("There are no events for that date range");
                    break;
                case 13:
                    Console.Clear();
                    CalendarsMenu(user, mediator);
                    break;
            }
        }
    }
    static void RemindersMenu(User user, IMediator mediator)
    {
        Console.WriteLine("Reminders Menu:");
        Console.WriteLine("1. View Reminders");
        Console.WriteLine("2. Add Reminder");
        Console.WriteLine("3. Edit Reminder");
        Console.WriteLine("4. Delete Reminder");
        Console.WriteLine("5. Back to Main Menu");
    }
    //done
    static async void CalendarsMenu(User user, IMediator mediator)
    {
        Console.WriteLine(user);
        IReadOnlyList<Calendar> user_calendars = null;// = await mediator.Send(new CalendarListQuery(user.Id));
        Calendar selected_calendar = null;

        while (true)
        {
            Console.WriteLine("Calendars Menu:");
            Console.WriteLine("1. View My Calendars");
            Console.WriteLine("2. Add Calendar");
            Console.WriteLine("3. Edit Calendar");
            Console.WriteLine("4. Delete Calendar");
            Console.WriteLine("5. Manage Calendar Events");
            Console.WriteLine("6. Manage calendar reminders");
            Console.WriteLine("7. Back to Main Menu");

            var choice = -1;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    user_calendars = await mediator.Send(new CalendarListQuery(user.Id));
                    Console.WriteLine("List of calendars");
                    foreach(var calendar in user_calendars)
                    {
                        Console.Write(calendar.Title, ", ");
                    }
                    break;
                case 2:
                    Console.WriteLine("Enter title: ");
                    string title = Console.ReadLine();
                    Console.WriteLine("Enter description: ");
                    string description = Console.ReadLine();
                    var new_calendar = await mediator.Send(
                        new CreateCalendarCommand(title, description, user.Id, new List<Event>(), new List<Reminder>()));
                    Console.WriteLine("Added new calendar!");
                    break;
                case 3:
                    Console.WriteLine("List of calendars");
                    foreach (var calendar in user_calendars)
                    {
                        Console.Write(calendar.Title, ", ");
                    }
                    Console.WriteLine("Enter name of the calendar to edit");
                    var name = Console.ReadLine();
                    var succes = false;
                    foreach(var calendar in user_calendars)
                    {
                        if(calendar.Title == name)
                        {
                            selected_calendar = calendar;
                            succes = true;
                        }
                    }
                    if (!succes)
                    {
                        Console.WriteLine("Cannot find calendar");
                        break;
                    }
                    Console.WriteLine("Enter new title:");
                    var new_title = Console.ReadLine();
                    Console.WriteLine("Enter new description:");
                    var new_desc = Console.ReadLine();
                    selected_calendar.Title = new_title;
                    selected_calendar.CalendarDescription = new_desc;

                    var updated_calendar = await mediator.Send(new UpdateCalendarCommand(selected_calendar.Id, selected_calendar));
                    break;
                case 4:
                    Console.WriteLine("List of calendars");
                    foreach (var calendar in user_calendars)
                    {
                        Console.Write(calendar.Title, ", ");
                    }
                    Console.WriteLine("Enter calendar name to delete");
                    title = Console.ReadLine();
                    succes = false;
                    foreach (var calendar in user_calendars)
                    {
                        if (calendar.Title == title)
                        {
                            selected_calendar = calendar;
                            succes = true;
                        }
                    }
                    if (!succes)
                    {
                        Console.WriteLine("Cannot find calendar");
                        break;
                    }
                    await mediator.Send(new DeleteCalendarCommand(selected_calendar.Id));
                    break;
                case 5:
                    Console.WriteLine("List of calendars:");
                    foreach (var calendar in user_calendars)
                    {
                        Console.WriteLine(calendar.Title, ", ");
                    }
                    Console.WriteLine();
                    Console.WriteLine("Enter calendar name: ");
                    title = Console.ReadLine();
                    succes = false;
                    foreach (var calendar in user_calendars)
                    {
                        if (calendar.Title == title)
                        {
                            selected_calendar = calendar;
                            succes = true;
                        }
                    }
                    if (!succes)
                    {
                        Console.WriteLine("Cannot find calendar");
                        break;
                    }
                    CalendarEventsMenu(user, selected_calendar, mediator);
                    break;
                case 6:
                    Console.Clear();
                    break;
                case 7:
                    Console.Clear();
                    break;
            }
        }
    }

}