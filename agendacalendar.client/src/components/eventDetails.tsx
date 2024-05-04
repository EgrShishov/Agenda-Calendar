import {useState, useContext} from "react";
import {Event} from '../models/eventModel.ts';
import {ReccurencyRule} from "../models/reccurencyRulePatternModel.ts";
import GlobalContext from "../context/globalContext.ts";
import {EventService} from "../services/eventService.ts";
import {useNavigate} from "react-router-dom";
import { enGB } from 'date-fns/locale'
import {DatePicker} from 'react-nice-dates'
import 'react-nice-dates/build/style.css';

const labelsClasses = [
    "indigo",
    "gray",
    "green",
    "blue",
    "red",
    "purple",
    "orange"
];

const EventDetails = () => {
    const {
        selectedEvent,
        setSelectedEvent,
        setShowEventDetails,
        calendarsList
    } = useContext(GlobalContext);

    const [title, setTitle] = useState(selectedEvent ? selectedEvent._def.title : '');
    const [description, setDescription] =
        useState(selectedEvent ? selectedEvent._def.extendedProps.description : '');

    const [startTime, setStartTime] = useState(selectedEvent ? selectedEvent.start : new Date());
    const [endTime, setEndTime] = useState(selectedEvent ? selectedEvent.end : new Date());
    const [location, setLocation] = useState(selectedEvent ? selectedEvent._def.location : '');

    const [isAllDay, setIsAllDay] = useState(false);
    const [participants, setParticipants] = useState([]);
    const [reccurenceRule, setReccurenceRule] = useState();
    const [selectedCalendar, setSelectedCalendar] =
        useState(selectedEvent ?
            calendarsList.find(calendar => calendar.id === selectedEvent._def.extendedProps.calendarId)
            : null
    );

    console.log(startTime, endTime);
    const [selectedLabel, setSelectedLabel] = useState(
        selectedEvent
            ? labelsClasses.find((lbl) => lbl === selectedEvent.label)
            : labelsClasses[0]
    );

    const Redirect = useNavigate();
    const eventService = new EventService();

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        const recRule: ReccurencyRule = {
            frequency: 0,
            interval: 0,
            daysOfWeek: [0],
            daysOfMonth: [0],
            weeksOfMonth: [0],
            monthsOfYear: [0],
            year: 0,
            recurrenceDates: [{startTime:new Date(), endTime: new Date()}],
        }
        const calendarEvent: Event = {
            title: title,
            description: description,
            startTime: new Date(startTime),
            endTime: new Date(endTime),
            location: location,
            recurrenceRule: recRule
        };
        if(!selectedEvent){
            console.log(selectedCalendar);
            const calendarId = selectedCalendar.id;
            const response = await eventService.createEvent(calendarEvent, calendarId);
            console.log(response);
        }

        setShowEventDetails(false);
    };

    const onDeleteHandle = async () => {
        const eventId = selectedEvent._def.publicId;
        const response = await eventService.deleteEvent(eventId);
        selectedEvent.remove();
        setShowEventDetails(false);
    };

    const onEditHandler = async () => {
        setShowEventDetails(false);
        Redirect(`/event/${selectedEvent._def.publicId}/edit`);
    };

    const eventIsAllDay = (event) =>{
        setIsAllDay(event.target.checked);
        const time = new Date(startTime.getTime());
        if (event.target.checked) {
            setEndTime(new Date(time.setDate(time.getDate() + 1)));
        } else {
            setEndTime(new Date(endTime.getTime()));
        }
    };

    const handleLabelChange = (event) => {
        setSelectedLabel(event.target.value);
    };

    const handleCalendarChange = (event) => {
        const selectedCalendarId = event.target.value;
        const selectedCalendar = calendarsList.find(calendar => calendar.id == selectedCalendarId);
        setSelectedCalendar(selectedCalendar);
    };

    const GetCalendarName = (calendarId) =>  {
        const calendar = calendarsList.find(calendar => calendar.id == calendarId);
        return calendar ? calendar.title : '';
    };

    return (
        <div className="my-14 w-full fixed h-screen flex justify-center items-center rounded-xl z-50">
            <form className="bg-white my-3 fixed w-4/12 h-fit rounded-xl shadow-2xl">
                <header className="rounded-t-xl bg-orange-300 px-4 py-2.5 flex justify-between items-center">
                      <span className="material-icons-outlined text-black-400">
                        drag_handle
                      </span>
                    <div>
                        {selectedEvent && (
                            <span
                                onClick={onEditHandler}
                                className="mx-2 material-icons-outlined font-light text-black-65 cursor-pointer"
                            >
                                    edit
                                </span>
                        )}
                        {selectedEvent && (
                            <span
                                onClick={onDeleteHandle}
                                className="mx-2 material-icons-outlined font-light text-black-65 cursor-pointer"
                            >
                                    delete
                                </span>
                        )}
                        <button onClick={() => setShowEventDetails(false)}>
                              <span className="mx-2 material-icons-outlined font-light text-black-65">
                                  close
                              </span>
                        </button>
                    </div>
                </header>
                <div className="p-3">
                    <div className="grid grid-cols-1/6 items-end gap-y-3.5">
                        <div></div>
                        <div className="row-span-1 flex items-center">
                            <span className="material-icons-outlined text-black-65">
                                subtitles
                            </span>
                            <input
                                type="text"
                                name="title"
                                placeholder="Add title"
                                value={title}
                                required
                                className="border-0 text-gray-600 text-xl mx-3 font-semibold pb-2
                                w-full border-b-2 border-gray-200 focus:outline-none focus:ring-0 focus:border-blue-500"
                                onChange={(e) => setTitle(e.target.value)}
                            />
                        </div>

                        <div className="row-span-1 flex items-center">
                            <span className="material-icons-outlined text-black-65">
                                schedule
                            </span>
                            {selectedEvent && (
                                <p className="mx-3 text-gary-400 text-ms font-medium">
                                    {startTime.toString()} - {startTime.toString()}
                                </p>
                            )}
                            {!selectedEvent && (
                                <div className="mx-3">
                                    <div className="flex items-center my-2">
                                        <span className="text-semibold text-black/60 text-ms mr-3">Start time:</span>
                                        <div style={{ position: 'relative', maxHeight: '300px'}}>
                                            <DatePicker
                                                date={startTime}
                                                onDateChange={setStartTime}
                                                format={"yyyy-MM-dd"}
                                                locale={enGB}>
                                                {({ inputProps, focused }) => (
                                                    <input
                                                        className={'input' + (focused ? ' -focused' : '')}
                                                        {...inputProps}
                                                        style={{ backgroundColor: "transparent"}}
                                                        placeholder="Select start time"
                                                    />
                                                )}
                                            </DatePicker>
                                        </div>
                                    </div>
                                    <div className="flex items-center my-2">
                                        <span className="text-semibold text-black/60 text-ms mr-3">End time:</span>
                                        <div style={{ position: 'relative', maxHeight: '200px'}}>
                                            <DatePicker
                                                date={endTime}
                                                onDateChange={setEndTime}
                                                format={"yyyy-MM-dd"}
                                                locale={enGB}>
                                                {({ inputProps, focused }) => (
                                                    <input
                                                        className={'input' + (focused ? ' -focused' : '')}
                                                        {...inputProps}
                                                        style={{ backgroundColor: "transparent"}}
                                                        placeholder="Select end time"
                                                    />
                                                )}
                                            </DatePicker>
                                        </div>
                                    </div>
                                    <div className="flex items-center my-2">
                                        <label className="text-semibold text-black/60 text-ms mr-3 flex items-center">
                                            All day
                                            <input
                                                checked={isAllDay}
                                                value={isAllDay}
                                                onChange={eventIsAllDay}
                                                type="checkbox"
                                                className="mx-2"
                                            />
                                        </label>
                                    </div>
                                </div>
                            )}
                        </div>

                        <div className="row-span-1 flex items-center">
                            <span className="material-icons-outlined text-black-65">
                                segment
                            </span>
                            {selectedEvent && (
                                <p className="mx-3 text-gary-400 text-ms font-medium">
                                    {description}
                                </p>
                            )}
                            {!selectedEvent && (
                                <input
                                    type="text"
                                    name="description"
                                    placeholder="Add description"
                                    value={description}
                                    required
                                    className="mx-3 border-0 text-gray-600 text-ms font-semibold pb-2 w-full border-b-2 border-gray-200 focus:outline-none focus:ring-0 focus:border-blue-500"
                                    onChange={(e) => setDescription(e.target.value)}
                                />
                            )}
                        </div>

                        <div className="row-span-1 flex items-center">
                            <span className="material-icons-outlined text-black-65">
                                location_on
                            </span>
                            {selectedEvent && (
                                <p className="mx-3 text-gary-400 text-ms font-medium">
                                    {location}
                                </p>
                            )}
                            {!selectedEvent && (
                                <input
                                    type="text"
                                    name="location"
                                    placeholder="Add location"
                                    value={location}
                                    required
                                    className="mx-3 border-0 text-gray-600 text-ms font-semibold pb-2 w-full border-b-2 border-gray-200 focus:outline-none focus:ring-0 focus:border-blue-500"
                                    onChange={(e) => setLocation(e.target.value)}
                                />
                            )}
                        </div>

                        <div className="row-span-1 flex items-center">
                            <span className="material-icons-outlined text-black-65">
                                notifications
                            </span>
                            {selectedEvent && (
                                <p className="mx-3 text-gary-400 text-ms font-medium">
                                    Notification
                                </p>
                            )}
                            {!selectedEvent && (
                                <div className="mx-3">
                                    RecRule pattern
                                </div>
                            )}
                        </div>

                        <div className="row-span-1 flex items-center">
                            <span className="material-icons-outlined text-black-65">
                                calendar_today
                            </span>
                            {selectedEvent && (
                                <p className="mx-3 text-gary-400 text-ms font-medium" >
                                    {GetCalendarName(selectedEvent._def.extendedProps.calendarId)}
                                </p>
                            )}
                            {!selectedEvent && (
                                <div className="flex gap-x-2 mx-3">
                                    <select value={selectedCalendar ? selectedCalendar.id : ''}
                                            onChange={handleCalendarChange}
                                            className="px-2 py-1 rounded border">
                                        <option value="">Select Calendar</option>
                                        {calendarsList.map(calendar => (
                                            <option
                                                key={calendar.id}
                                                value={calendar.id}
                                            >
                                                {calendar.title}
                                            </option>
                                        ))}
                                    </select>
                                    {
                                        labelsClasses.map((lblClass, i) => (
                                            <span
                                                key={i}
                                                onClick={() => setSelectedLabel(lblClass)}
                                                className={`w-6 h-6 rounded-full bg-${lblClass}-400 flex items-center justify-center cursor-pointer`}
                                            >
                                            {selectedLabel === lblClass && (
                                                <span className="material-icons-outlined text-white text-sm">
                                                    check
                                                </span>
                                            )}
                                        </span>))
                                    }
                                </div>
                            )}
                        </div>

                        {!selectedEvent && (
                            <button
                                type="submit"
                                onClick={handleSubmit}
                                className="bg-orange-300 hover:bg-black/60 px-6 py-2 rounded text-white
                                            hover:scale-105 transition ease-out duration-200 transform"
                            >
                                Add new event
                            </button>
                        )}
                    </div>
                </div>
            </form>
        </div>
    );
};

export default EventDetails;