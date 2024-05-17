import React, {useEffect, useMemo, useRef, useState} from 'react';
import GlobalContext from './globalContext.ts';
import calendarList from "../components/calendarList.tsx";
import {CalendarService} from "../services/calendarService.ts";
import {UserService} from "../services/userService.ts";
import {EventService} from "../services/eventService.ts";
import {ReminderService} from "../services/reminderService.ts";

const allColors = [
    "indigo",
    "gray",
    "green",
    "blue",
    "red",
    "purple",
    "orange",
    "cyan",
    "magenta",
    "yellow",
    "lime",
    "teal",
    "pink",
    "lavender",
    "brown",
    "olive",
    "navy",
    "maroon",
    "black",
    "white",
    "gold",
    "silver",
    "coral",
    "skyblue",
    "violet",
    "tan",
    "orchid",
    "salmon",
    "seagreen",
    "tomato"
];

export default function ContextWrapper(props){

    const calendarRef = useRef(null);
    const [calendarsList, setCalendarsList] = useState([]);
    const [events, setEvents] = useState([]);
    const [showEventDetails, setShowEventDetails] = useState(false);
    const [showCalendarModal, setShowCalendarModal] = useState(false);
    const [showReminderModal, setShowReminderModal] = useState(false);
    const [selectedEvent, setSelectedEvent] = useState(null);
    const [labels, setLabels] = useState([]);

    const [labelsClasses, setLabelsClasses] = useState(allColors);
    const [usedColors, setUsedColors] = useState([]); //used calendars and their colors

    const calendarService = new CalendarService();
    const userService = new UserService();
    const eventService = new EventService();
    const reminderService = new ReminderService();


    useEffect(() => {
        const availableColors = allColors.filter(color => !usedColors.includes(color));
        setLabelsClasses(availableColors);
    }, [usedColors]);

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

                const usedColors = calendars.map(calendar => calendar.calendarColor);
                setUsedColors(usedColors);

                console.log(usedColors);

            } catch (error) {
                console.error('Error fetching calendars:', error);
            }
        };
        fetchCalendars();
    },[]);

    useEffect(() => {
        if (!showEventDetails) {
            setSelectedEvent(null);
        }
    }, [showEventDetails]);

    function updateLabel(obj){
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
                calendarRef: calendarRef,
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