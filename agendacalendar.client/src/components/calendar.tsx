import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-icons/font/bootstrap-icons.css';

import FullCalendar from '@fullcalendar/react'
import dayGridPlugin from '@fullcalendar/daygrid'
import bootstrap5Plugin from '@fullcalendar/bootstrap5';
import {CalendarService} from "../services/calendarService.ts";
import {useEffect, useState} from "react";

const Calendar = () => {
    const [events, setEvents] = useState([])
    const calendarService = new CalendarService();

    useEffect(() => {
        const fetchEvents = async () => {
            const events = await calendarService.getCalendars(13);
            setEvents(events);
        };
        fetchEvents()},
[]);

    return (
        <div className={"w-screen overflow-x-hidden"}>
            <FullCalendar
                plugins={[dayGridPlugin, bootstrap5Plugin]}
                initialView="dayGridMonth"
                headerToolbar={{
                    end: 'today,dayGridWeek,dayGridMonth',
                    //end: '',
                    center: 'title',
                    //start: ''
                    start: 'prev,next'
                }}
                events={events}
                themeSystem={"bootstrap5"}
            />
        </div>
    );
};

export default Calendar;