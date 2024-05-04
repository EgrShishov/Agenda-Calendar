import React, {useEffect, useState} from 'react';
import GlobalContext from './globalContext.ts';
import calendarList from "../components/calendarList.tsx";
import {CalendarService} from "../services/calendarService.ts";
import {UserService} from "../services/userService.ts";
import {EventService} from "../services/eventService.ts";
import {ReminderService} from "../services/reminderService.ts";

export default function ContextWrapper(props){
    const [calendarsList, setCalendarsList] = useState([]);
    const [showEventDetails, setShowEventDetails] = useState(false);
    const [showCalendarModal, setShowCalendarModal] = useState(false);
    const [showReminderModal, setShowReminderModal] = useState(false);
    const [selectedEvent, setSelectedEvent] = useState(null);
    const [labels, setLabels] = useState([]);
    const [filteredEvents, setFilteredEvents] = useState([]);
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    const calendarService = new CalendarService();
    const userService = new UserService();
    const eventService = new EventService();
    const reminderService = new ReminderService();

    useEffect(() => {
        const fetchCalendars = async () => {
            try {
                const calendars = await calendarService.getCalendars();
                setCalendarsList(calendars);
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

    useEffect(() => {
        setLabels((prevLabels) => {
            return [...new Set(calendarsList.map((cal) => cal.label))].map(
                (label) => {
                    const currentLabel = prevLabels.find(
                        (lbl) => lbl.label === label
                    );
                    return {
                        label,
                        checked: currentLabel ? currentLabel.checked : true,
                    };
                }
            );
        });
    }, [calendarsList]);

    function updateLabel(label){
        setLabels(
            calendarsList.map((lbl) => (lbl.label === label.label ? label : lbl))
        );
    }

    return (
        <GlobalContext.Provider
            value={{
                isAuthenticated,
                setIsAuthenticated,
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
                filteredEvents,
                setFilteredEvents,
                calendarsList,
                setCalendarsList
            }}
        >
            { props.children }
        </GlobalContext.Provider>
    );
}