import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-icons/font/bootstrap-icons.css';
import FullCalendar from '@fullcalendar/react'
import dayGridPlugin from '@fullcalendar/daygrid'
import bootstrap5Plugin from '@fullcalendar/bootstrap5';
import {CalendarService} from "../services/calendarService.ts";
import {useContext, useEffect, useState} from "react";
import GlobalContext from "../context/globalContext.ts";
import {EventService} from "../services/eventService.ts";


const Calendar = () => {
    const calendarService = new CalendarService();
    const eventService = new EventService();
    const {setSelectedEvent, setShowEventDetails} = useContext(GlobalContext);
    const {filteredEvents, events, setEvents} = useContext(GlobalContext);

    const onEventClickHandler = (clickInfo) => {
        setSelectedEvent(clickInfo.event)
        setShowEventDetails(true);
    };

    useEffect(() => {
        const fetchEvents = async () => {
            const events = await eventService.getUserEvents();
            setEvents(events);
            console.log(events);
        };
        fetchEvents()},
[]);

    return (
        <div className="container max-w-screen-ms max-h-screen-md mt-4">
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
                events={filteredEvents}
                eventClick={onEventClickHandler}
                themeSystem={"bootstrap5"}
            />
        </div>
    );
};

export default Calendar;