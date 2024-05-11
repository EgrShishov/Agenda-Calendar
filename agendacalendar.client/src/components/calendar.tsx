import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-icons/font/bootstrap-icons.css';
import FullCalendar from '@fullcalendar/react';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import bootstrap5Plugin from '@fullcalendar/bootstrap5';
import rrulePlugin from '@fullcalendar/rrule';
import {useContext, useEffect} from "react";
import GlobalContext from "../context/globalContext.ts";
import {EventService} from "../services/eventService.ts";


const Calendar = () => {
    const {calendarRef} = useContext(GlobalContext);
    const eventService = new EventService();

    const {setSelectedEvent, setShowEventDetails} = useContext(GlobalContext);
    const { filteredEvents, events, setEvents} = useContext(GlobalContext);

    const onEventClickHandler = (clickInfo) => {
        setSelectedEvent(clickInfo.event)
        setShowEventDetails(true);
    };

    useEffect(() => {
        const fetchEvents = async () => {
            const events = await eventService.getUserEvents();
            setEvents(events);
        };
        fetchEvents()},
[events]);

    return (
        <div className="container max-w-screen-ms max-h-screen-md mt-4">
            <FullCalendar
                ref={calendarRef}
                plugins={[dayGridPlugin, timeGridPlugin, bootstrap5Plugin, rrulePlugin]}
                initialView="dayGridMonth"
                headerToolbar={false}
                height={630}
                events={filteredEvents}
                eventClick={onEventClickHandler}
            />
        </div>
    );
};

export default Calendar;