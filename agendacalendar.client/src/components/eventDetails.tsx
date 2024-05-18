import React, {useState, useContext, useEffect} from "react";
import {Event, RecurrenceRule} from '../models/eventModel.ts';
import GlobalContext from "../context/globalContext.ts";
import {EventService} from "../services/eventService.ts";
import { enGB } from 'date-fns/locale'
import {DatePicker} from 'react-nice-dates'
import 'react-nice-dates/build/style.css';
import {Button, MenuItem, Menu} from '@mui/material';
import ReccurecyRuleModal from "./reccurecyRuleModal.tsx";
import {format, isToday} from 'date-fns';

const EventDetails = () => {
    const {
        labelsClasses,
        events,
        setEvents,
        selectedEvent,
        setSelectedEvent,
        setShowEventDetails,
        calendarsList
    } = useContext(GlobalContext);

    console.log('event details', selectedEvent);

    const [anchorEl, setAnchorEl] = useState(null);
    const [selectedItem, setSelectedItem] = useState(null);
    const [editingMode, setEditingMode] = useState(false);

    const handleClick = (event) => {
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };


    const [title, setTitle] = useState(selectedEvent ? (selectedEvent.title? selectedEvent.title : '') : '');
    const [description, setDescription] =
        useState(selectedEvent ?
            (selectedEvent.extendedProps.description ? selectedEvent.extendedProps.description : '') : '');

    const emptyReccurenceRule : RecurrenceRule = {
        interval: 0,
        freq: 'none',
        byweekday: [],
        dtstart: '',
        until: ''
    }

    const [reccurenceRule, setReccurenceRule] = useState(emptyReccurenceRule);

    function convertFrequency(freq){
        switch(freq){
            case 0:
                return 'none';
            case 1:
                return 'daily';
            case 2:
                return 'weekly';
            case 3:
                return 'monthly';
            case 4:
                return 'yearly';
        }
    }

    function convertWeekdays(daysArray){
        const daysMapping = {
            0: 'su',
            1: 'mo',
            2: 'tu',
            3: 'we',
            4: 'th',
            5: 'fr',
            6: 'sa'
        };
        return daysArray != null ? daysArray.map(day => daysMapping[day]) : [];
    }

    useEffect(()=>{
        if(selectedEvent) {
            const recrule = selectedEvent._def.recurringDef ? selectedEvent._def.recurringDef.typeData.rruleSet._rrule[0].options : null;
            let rrule = null;

            if (recrule) {
                rrule = {
                    freq: convertFrequency(recrule.freq) ?? 'none',
                    interval: recrule.interval ?? 0,
                    byweekday: convertWeekdays(recrule.byweekday) ?? [''],
                    dtstart: recrule.dtstart ?? '',
                    until: recrule.until ?? new Date()
                };
                setReccurenceRule(rrule);
            }
        }
    },[selectedEvent]);

    const [startTime, setStartTime] = useState(selectedEvent ? selectedEvent.start : new Date());
    const [endTime, setEndTime] = useState(selectedEvent ? selectedEvent.end : new Date());
    const [location, setLocation] = useState(selectedEvent ?
        (selectedEvent.extendedProps.resourceId ? selectedEvent.extendedProps.resourceId : '') : '');

    const [isAllDay, setIsAllDay] = useState(false);

    const [participants, setParticipants] = useState([]);

    const [selectedCalendar, setSelectedCalendar] =
        useState(selectedEvent ?
            calendarsList.find(calendar => calendar.calendar.id === selectedEvent.extendedProps.calendarId)
            : null
        );

    const [notifications, setNotifications] = useState();
    const [showRecurrenceModal, setShowRecurrenceModal] = useState(false);
    const [bgColor, setBgColor] = useState(selectedEvent? selectedEvent.backgroundColor : 'orange');

    const eventService = new EventService();

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        console.log(reccurenceRule);

        const calendarEvent: Event = {
            title: title,
            description: description,
            startTime: startTime,
            endTime: endTime,
            location: location,
            recurrenceRule: reccurenceRule
        };

        if(!selectedEvent)
        {
            const calendarId = selectedCalendar.calendar.id;
            const response = await eventService.createEvent(calendarEvent, calendarId);
            events.push(calendarEvent);
            setEvents(events);
        }
        else if(editingMode)
        {
            const eventId = selectedEvent._def.publicId;
            const calendarId = selectedEvent._def.extendedProps.calendarId;

            /*selectedEvent.setProp('title', title);
            selectedEvent.setExtendedProp('description', description);
            selectedEvent.setStart(new Date(startTime));
            selectedEvent.setEnd(new Date(endTime));*/

            const response = await eventService.editEvent(calendarEvent, eventId, calendarId);
            console.log(response);
        }

        setShowEventDetails(false);
    };

    const onDeleteHandle = async () => {
        const eventId = selectedEvent._def.publicId;
        const response = await eventService.deleteEvent(eventId);

        setShowEventDetails(false);
        selectedEvent.remove();
    };

    const onEditHandler = async () => {
        setEditingMode(true);
        setSelectedCalendar(calendarsList.find(calendar => calendar.calendar.id === selectedEvent.extendedProps.calendarId));
    };

    const handleDiscard = () => {
        setEditingMode(false);
    }

    const eventIsAllDay = (event) =>{
        setIsAllDay(event.target.checked);
        const time = new Date(startTime.getTime());

        if (event.target.checked) {
            setEndTime(new Date(time.setDate(time.getDate() + 1)));
        } else {
            setEndTime(new Date(endTime.getTime()));
        }
    };

    const handleCalendarChange = (event) => {
        const selectedCalendarId = event.target.value;
        const selectedCalendar = calendarsList.find(calendar => calendar.calendar.id == selectedCalendarId);
        setSelectedCalendar(selectedCalendar);
    };

    const GetCalendarName = (calendarId) =>  {
        const calendar = calendarsList.find(calendar => calendar.calendar.id == calendarId);
        return calendar.calendar ? calendar.calendar.title : '';
    };

    const startDateChanged = (newTime) => {
        const parsedNewStartTime = new Date(newTime);
        const parsedEndTime = new Date(endTime);

        if (parsedNewStartTime.getTime() > parsedEndTime.getTime()) {
            const newEndTime = new Date(parsedNewStartTime);
            newEndTime.setDate(newEndTime.getDate() + 1);
            setEndTime(newEndTime);
        }

        setStartTime(parsedNewStartTime);
    };

    const endDateChanged = (newTime) => {
        const parsedEndTime = new Date(newTime);
        const parsedStartTime = new Date(startTime);
        const now = new Date();

        if (parsedEndTime.getTime() < parsedStartTime.getTime()) {
            const newStartTime = new Date(parsedEndTime);
            newStartTime.setDate(newStartTime.getDate() - 1);
            setStartTime(newStartTime);
        }

        setEndTime(parsedEndTime);
    };

    const handleItem = (item) => {
        setSelectedItem(item);
        setBgColor(item.backgroundColor);

        if(item == 'none'){
            console.log(item);
        } else if(item == 'custom'){
            setShowRecurrenceModal(true);
        }
        handleClose();
    };

    const formatDate = (dateString) => {
        return format(new Date(dateString), 'MMMM d, yyyy');
    };

    const formatWeekdays = (byweekday) => {
        console.log(byweekday);
        const daysOfWeek = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
        return byweekday.map((day, idx) => daysOfWeek[idx]).join(', ');
    };

    const formatRecurrencePattern = (recurrenceRule) => {
        const { freq, interval, byweekday, dtstart, until } = recurrenceRule;

        let pattern = `Repeats in ${interval} time(s) every `;
        switch (freq) {
            case 'daily':
                pattern += 'month';
                break;
            case 'weekly':
                pattern += 'week';
                break;
            case 'monthly':
                pattern += 'day';
                break;
            case 'yearly':
                pattern += 'year';
                break;
            default:
                pattern += freq;
                break;
        }

        if (byweekday && byweekday.length > 0) {
            pattern += `, on ${formatWeekdays(byweekday)}`;
        }
        pattern += `, starting from ${formatDate(dtstart)}`;

        if (until) {
            pattern += `, ending on ${formatDate(until)}`;
        }
        return pattern;
    };


    return (
        <React.Fragment>
            {
                (!selectedEvent && showRecurrenceModal) ||
                (selectedEvent && showRecurrenceModal)
                ? (
                    <ReccurecyRuleModal setShowReccurenceModal={setShowRecurrenceModal}
                                        setReccurenceRule={setReccurenceRule} />
                ) : null
            }
            <div className="my-14 w-full fixed h-screen flex justify-center items-center rounded-xl z-10">
                <form className="bg-white my-3 fixed w-4/12 h-fit rounded-xl shadow-2xl">
                    <header className={`rounded-t-xl px-4 py-2.5 flex justify-between items-center`}
                        style={{ backgroundColor: selectedCalendar ? selectedCalendar.calendar.calendarColor :
                                (selectedEvent ? selectedEvent.backgroundColor : 'orange') }}>
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
                                <div>
                                    {(editingMode || !selectedEvent) ? (
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
                                    ) : (
                                        <span
                                            className="border-0 text-gray-600 text-xl mx-3 font-semibold pb-2
                                            w-full border-b-2 border-gray-200 focus:outline-none focus:ring-0 focus:border-blue-500"
                                        >
                                            {title}
                                        </span>
                                    )}
                                </div>
                            </div>

                            <div className="row-span-1 flex items-center">
                                <span className="material-icons-outlined text-black-65">
                                    schedule
                                </span>
                                {(!selectedEvent || editingMode) ? (
                                    <div className="mx-3">
                                        <div className="flex items-center my-2">
                                            <span
                                                className="text-semibold text-black/60 text-ms mr-3">Start time:</span>
                                            <div style={{position: 'relative', maxHeight: '300px'}}>
                                                <DatePicker
                                                    date={startTime}
                                                    minimumDate={Date.now()}
                                                    onDateChange={startDateChanged}
                                                    format={"yyyy-MM-dd HH:mm"}
                                                    locale={enGB}>
                                                    {({inputProps, focused}) => (
                                                        <input
                                                            className={'input' + (focused ? ' -focused' : '')}
                                                            {...inputProps}
                                                            style={{backgroundColor: "transparent"}}
                                                            placeholder="Select start date & time"
                                                        />
                                                    )}
                                                </DatePicker>
                                            </div>
                                        </div>
                                        <div className="flex items-center my-2">
                                            <span className="text-semibold text-black/60 text-ms mr-3">End time:</span>
                                            <div style={{position: 'relative', maxHeight: '200px'}}>
                                                <DatePicker
                                                    date={endTime}
                                                    minimumDate={Date.now()}
                                                    onDateChange={endDateChanged}
                                                    format={"yyyy-MM-dd HH:mm"}
                                                    locale={enGB}>
                                                    {({inputProps, focused}) => (
                                                        <input
                                                            className={'input' + (focused ? ' -focused' : '')}
                                                            {...inputProps}
                                                            style={{backgroundColor: "transparent"}}
                                                            placeholder="Select end date & time"
                                                        />
                                                    )}
                                                </DatePicker>
                                            </div>
                                        </div>
                                        <div className="flex items-center my-2">
                                            <label
                                                className="text-semibold text-black/60 text-ms mr-3 flex items-center">
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
                                ) : (
                                    <span className="mx-3 text-gary-400 text-ms font-medium">
                                        {startTime.toString()} - {endTime && (endTime.toString())}
                                    </span>
                                )}
                            </div>

                            <div className="row-span-1 flex items-center">
                                <span className="material-icons-outlined text-black-65">
                                    segment
                                </span>
                                {(!selectedEvent || editingMode) ? (
                                    <input
                                        type="text"
                                        name="description"
                                        placeholder="Add description"
                                        value={description}
                                        required
                                        className="mx-3 border-0 text-gray-600 text-ms font-semibold pb-2 w-full border-b-2 border-gray-200 focus:outline-none focus:ring-0 focus:border-blue-500"
                                        onChange={(e) => setDescription(e.target.value)}
                                    />
                                ): (
                                    <span
                                        className="mx-3 text-gary-400 text-ms font-medium"
                                    >
                                        {description}
                                    </span>
                                )}
                            </div>

                            {(location != '' || editingMode) && (
                                <div className="row-span-1 flex items-center">
                                <span className="material-icons-outlined text-black-65">
                                    location_on
                                </span>
                                    {(!selectedEvent || editingMode) ? (
                                        <input
                                            type="text"
                                            name="location"
                                            placeholder="Add location"
                                            value={location}
                                            required
                                            className="mx-3 border-0 text-gray-600 text-ms font-semibold pb-2 w-full border-b-2 border-gray-200 focus:outline-none focus:ring-0 focus:border-blue-500"
                                            onChange={(e) => setLocation(e.target.value)}
                                        />
                                    ) : (
                                        <span className="mx-3 text-gary-400 text-ms font-medium">
                                        {location}
                                    </span>
                                    )}
                                </div>
                            )}

                            {(reccurenceRule.freq !== 'none' || editingMode || !selectedEvent) && (
                                <div className="row-span-1 flex items-center">
                                <span className="material-icons-outlined text-black-65">
                                    repeat
                                </span>
                                    {(!selectedEvent || editingMode) ? (
                                        <div className="mx-3">
                                            <Button
                                                className="bg-white border p-2"
                                                aria-controls="simple-menu"
                                                aria-haspopup="true"
                                                onClick={handleClick}
                                            >
                                              <span className="mx-2 font-semibold text-ms text-black/80">
                                                {selectedItem ? selectedItem : "Add pattern"}
                                              </span>
                                            </Button>
                                            <Menu
                                                anchorEl={anchorEl}
                                            keepMounted
                                            open={Boolean(anchorEl)}
                                            onClose={handleClose}
                                            transformOrigin={{
                                                vertical: 'top',
                                                horizontal: 'left',
                                            }}
                                        >
                                            <MenuItem onClick={() => handleItem('none')}>None</MenuItem>
                                            <MenuItem onClick={() => handleItem('custom')}>Custom</MenuItem>
                                        </Menu>
                                        </div>
                                        ) : (
                                            <span className="mx-3 text-gary-400 text-ms font-medium">
                                               {formatRecurrencePattern(reccurenceRule)}
                                            </span>
                                        )}
                                </div>
                            )}

                            {selectedEvent && notifications && (
                                <div className="row-span-1 flex items-center">
                                <span className="material-icons-outlined text-black-65">
                                    notifications
                                </span>
                                    {selectedEvent && (
                                        <p className="mx-3 text-gary-400 text-ms font-medium">
                                            {notifications} За 30 минут до начала
                                        </p>
                                    )}
                                </div>
                            )}

                            <div className="row-span-1 flex items-center">
                                <span className="material-icons-outlined text-black-65">
                                    calendar_today
                                </span>
                                {(!selectedEvent) ? (
                                    <div className="flex gap-x-2 mx-3">
                                        <select value={selectedCalendar ? selectedCalendar.id : ''}
                                                onChange={handleCalendarChange}
                                                className="px-2 py-1 rounded border bg-white">
                                            {!selectedEvent && (
                                                <option value="" disabled>Select Calendar</option>
                                            )}
                                            {calendarsList.map(calendar => (
                                                <option
                                                    key={calendar.calendar.id}
                                                    value={calendar.calendar.id}
                                                    color={calendar.calendar.calendarColor}
                                                    className="flex items-center"
                                                >
                                                    {calendar.calendar.title}
                                                </option>
                                            ))}
                                        </select>
                                    </div>
                                ) : (
                                    <span className="mx-3 text-gary-400 text-ms font-medium">
                                        {GetCalendarName(selectedEvent.extendedProps.calendarId)}
                                    </span>
                                )}
                            </div>
                            
                            {!selectedEvent && (
                                <button
                                    type="submit"
                                    onClick={handleSubmit}
                                    style={{ backgroundColor: selectedCalendar ? selectedCalendar.calendar.calendarColor :
                                            (selectedEvent ? selectedEvent.backgroundColor : 'orange') }}
                                    className="hover:bg-black/60 px-6 py-2 rounded text-black
                                            hover:scale-105 transition ease-out duration-200 transform"
                                >
                                    Add new event
                                </button>
                            )}
                            {selectedEvent && editingMode && (
                                <div className="row-span-1 flex justify-content-end gap-x-2.5">
                                    <button
                                        type="submit"
                                        onClick={handleSubmit}
                                        style={{ backgroundColor: selectedCalendar ? selectedCalendar.calendar.calendarColor :
                                                (selectedEvent ? selectedEvent.backgroundColor : 'orange') }}
                                        className="hover:bg-black/60 px-6 py-2 rounded text-black
                                            hover:scale-105 transition ease-out duration-200 transform"
                                    >
                                        Save changes
                                    </button>
                                    <button
                                        onClick={handleDiscard}
                                        style={{ backgroundColor: selectedCalendar ? selectedCalendar.calendar.calendarColor :
                                                (selectedEvent ? selectedEvent.backgroundColor : 'orange') }}
                                        className="hover:bg-black/60 px-6 py-2 rounded text-black
                                                hover:scale-105 transition ease-out duration-200 transform"
                                    >
                                        Discard
                                    </button>
                                </div>
                            )}
                        </div>
                    </div>
                </form>
            </div>
        </React.Fragment>
    );
};

export default EventDetails;