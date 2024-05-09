import React, {useEffect, useMemo, useState} from 'react';
import GlobalContext from './globalContext.ts';
import calendarList from "../components/calendarList.tsx";
import {CalendarService} from "../services/calendarService.ts";
import {UserService} from "../services/userService.ts";
import {EventService} from "../services/eventService.ts";
import {ReminderService} from "../services/reminderService.ts";

const labelsClasses = [
    "indigo",
    "gray",
    "green",
    "blue",
    "red",
    "purple",
    "orange"
];

export default function ContextWrapper(props){

    const [calendarsList, setCalendarsList] = useState([]);
    const [events, setEvents] = useState([]);
    const [showEventDetails, setShowEventDetails] = useState(false);
    const [showCalendarModal, setShowCalendarModal] = useState(false);
    const [showReminderModal, setShowReminderModal] = useState(false);
    const [selectedEvent, setSelectedEvent] = useState(null);
    const [labels, setLabels] = useState([]);
    const [isAuthenticated, setIsAuthenticated] = useState(JSON.parse(localStorage.getItem('authState')));

    const calendarService = new CalendarService();
    const userService = new UserService();
    const eventService = new EventService();
    const reminderService = new ReminderService();

    useEffect(() => {
        const storedAuthState = localStorage.getItem('authState');
        if (storedAuthState) {
            setIsAuthenticated(JSON.parse(storedAuthState));
        }
    }, []);

    const handleSetIsAuthenticated = (value) => {
        setIsAuthenticated(value);
        localStorage.setItem('authState', JSON.stringify(value));
    };

    useEffect(() => {
        const fetchCalendars = async () => {
            try {
                const calendars = await calendarService.getCalendars();
                setCalendarsList(calendars.map((calendar) => {
                    return {
                        calendar,
                        checked: true
                    }
                }));
            } catch (error) {
                console.error('Error fetching calendars:', error);
            }
        };
        fetchCalendars();
    }, []);

    useEffect(() => {
        if (!showEventDetails) {
            setSelectedEvent(null);
        }
    }, [showEventDetails]);

    function updateLabel(obj){
        console.log(obj);
        console.table(calendarsList);
        setCalendarsList(calendarsList.map((calobj) =>  {
                const calendarLabel = {
                    label: calobj.calendar.calendarColor,
                    checked: calobj.checked
                };
                if (calendarLabel.label == obj.label){
                    calobj.checked = obj.checked;
                }
                return calobj;
            }
        ));
        console.table(calendarsList);
    }

    const filteredEvents = useMemo(() => {
        return events.filter((event) =>
            calendarsList
                .filter((obj) => obj.checked)
                .map((obj) => obj.calendar.calendarColor)
                .includes(event.backgroundColor)
        );
    }, [events, calendarsList]);

    return (
        <GlobalContext.Provider
            value={{
                isAuthenticated,
                setIsAuthenticated: handleSetIsAuthenticated,
                showEventDetails,
                setShowEventDetails,
                showCalendarModal,
                setShowCalendarModal,
                showReminderModal,
                setShowReminderModal,
                selectedEvent,
                setSelectedEvent,
                setLabels,
                labels,
                updateLabel,
                events,
                setEvents,
                filteredEvents,
                calendarsList,
                setCalendarsList,
                labelsClasses,
            }}
        >
            { props.children }
        </GlobalContext.Provider>
    );
}