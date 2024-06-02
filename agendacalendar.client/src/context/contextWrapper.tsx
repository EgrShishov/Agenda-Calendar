import React, {useEffect, useMemo, useRef, useState} from 'react';
import GlobalContext from './globalContext.ts';
import calendarList from "../components/calendarList.tsx";
import {CalendarService} from "../services/calendarService.ts";
import {UserService} from "../services/userService.ts";
import {EventService} from "../services/eventService.ts";
import {ReminderService} from "../services/reminderService.ts";
import {useCookies} from "react-cookie";

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
    const [sharedCalendarsList, setSharedCalendarsList] = useState([]);
    const [events, setEvents] = useState([]);
    const [showEventDetails, setShowEventDetails] = useState(false);
    const [showCalendarModal, setShowCalendarModal] = useState(false);
    const [showReminderModal, setShowReminderModal] = useState(false);
    const [showWorkingHoursModal, setShowWorkingHoursModal] = useState(false);
    const [selectedEvent, setSelectedEvent] = useState(null);
    const [labels, setLabels] = useState([]);
    const [meetings, setMeetings] = useState([]);
    const [showSuggestModal, setShowSuggestModal] = useState(false);

    const [labelsClasses, setLabelsClasses] = useState(allColors);
    const [usedColors, setUsedColors] = useState([]);

    useEffect(() => {
        if (!showEventDetails) {
            setSelectedEvent(null);
        }
    }, [showEventDetails]);

    useEffect(() => {
        const availableColors = allColors.filter(color => !usedColors.includes(color));
        setLabelsClasses(availableColors);
    }, [usedColors, calendarsList]);

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
    }

    const filteredEvents = useMemo(() => {
        const allCalendarsList = [...calendarsList, ...sharedCalendarsList];

        return events.filter((event) =>
            allCalendarsList
                .filter((obj) => obj.checked)
                .map((obj) => obj.calendar.calendarColor)
                .includes(event.backgroundColor)
        );

    }, [events, calendarsList, sharedCalendarsList]);

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
                showWorkingHoursModal,
                setShowWorkingHoursModal,
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
                sharedCalendarsList,
                setSharedCalendarsList,
                labelsClasses,
                meetings,
                setMeetings,
                showSuggestModal,
                setShowSuggestModal,
                setUsedColors
            }}
        >
            { props.children }
        </GlobalContext.Provider>
    );
}