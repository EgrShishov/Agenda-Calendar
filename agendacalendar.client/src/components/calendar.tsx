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
import {RecurrenceRule} from "../models/eventModel.ts";


const Calendar = () => {
    const {calendarRef} = useContext(GlobalContext);
    const eventService = new EventService();

    const {setSelectedEvent, selectedEvent, setShowEventDetails} = useContext(GlobalContext);
    const { filteredEvents, events, setEvents} = useContext(GlobalContext);

    const onEventClickHandler = (clickInfo) => {
        const event = clickInfo.event;

        const rrule = event._def.recurringDef ? event._def.recurringDef.typeData.rruleSet._rrule[0].options : null;
        console.log(rrule);
        const selectedEventData = {
            title: event.title,
            description: event._def.extendedProps.description,
            start: event.start,
            end: event.end,
            location: event.resourceId,
            backgroundColor: event.backgroundColor,
            calendarId: event._def.extendedProps.calendarId,
            publicId: event._def.publicId,
            rrule: null
        };

        if(rrule){
            selectedEventData.rrule = {
                freq: rrule.freq,
                interval: rrule.interval,
                byweekday: rrule.byweekday,
                dtstart: rrule.dtstart,
                untill: rrule.untill
            }
        }
        setSelectedEvent(selectedEventData);
        setShowEventDetails(true);
    };

    useEffect(() => {
        const fetchEvents = async () => {
            const events = await eventService.getUserEvents();
            setEvents(events);
        };
        fetchEvents();
        },
[events]);
    
    return (
        <div className="container max-w-screen-ms max-h-screen-md mt-4">
            <FullCalendar
                ref={calendarRef}
                plugins={[dayGridPlugin, timeGridPlugin, bootstrap5Plugin, rrulePlugin]}
                initialView="dayGridMonth"
                firstDay={1}
                headerToolbar={false}
                height={630}
                events={filteredEvents}
                eventClick={onEventClickHandler}
            />
        </div>
    );
};

export default Calendar;