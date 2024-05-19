import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-icons/font/bootstrap-icons.css';
import FullCalendar from '@fullcalendar/react';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import timeGridPlugin from '@fullcalendar/timegrid';
import bootstrap5Plugin from '@fullcalendar/bootstrap5';
import rrulePlugin from '@fullcalendar/rrule';
import {useContext, useEffect} from "react";
import GlobalContext from "../context/globalContext.ts";
import {EventService} from "../services/eventService.ts";
import {Event, RecurrenceRule} from '../models/eventModel.ts';

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

const Calendar = () => {
    const eventService = new EventService();

    const {
        calendarRef,
        setSelectedEvent,
        selectedEvent,
        setShowEventDetails,
        filteredEvents,
        setEvents,
        events
    } = useContext(GlobalContext);

    const onEventClickHandler = (clickInfo) => {
        setSelectedEvent(clickInfo.event);
        setShowEventDetails(true);
    };

    useEffect(() => {
        const fetchEvents = async () => {
            const events = await eventService.getUserEvents();
            setEvents(events);
        };
        fetchEvents();
    }, []);

    const handleEventChange = (info) => {
        const event = info.event;
        const recrule = event._def.recurringDef ? event._def.recurringDef.typeData.rruleSet._rrule[0].options : null;
        let rrule : RecurrenceRule = {
            interval: 0,
            freq: 'none',
            byweekday: [],
            dtstart: '',
            until: ''
        }

        if (recrule) {
            rrule = {
                freq: convertFrequency(recrule.freq) ?? 'none',
                interval: recrule.interval ?? 0,
                byweekday: convertWeekdays(recrule.byweekday) ?? [''],
                dtstart: recrule.dtstart ?? '',
                until: recrule.until ?? new Date()
            };
        }
        const updatedEvent : Event = {
            title: event.title,
            description: event.extendedProps.description,
            location: event.extendedProps.resourceId ?? '',
            startTime: info.event.start,
            endTime: info.event.end,
            recurrenceRule: rrule ?? ''
        };

        const eventId = event._def.publicId;
        const calendarId = event.extendedProps.calendarId;

        const updateEvents = async (updatedEvent, eventId, calendarId) =>
        {
            const response = await eventService.editEvent(updatedEvent, eventId, calendarId);
        }

        updateEvents(updatedEvent, eventId, calendarId);
    }

    return (
        events ? (
            <div className="container max-w-screen-ms max-h-screen-md mt-4">
                <FullCalendar
                    ref={calendarRef}
                    plugins={[dayGridPlugin, timeGridPlugin, bootstrap5Plugin, rrulePlugin, interactionPlugin]}
                    initialView="dayGridMonth"
                    firstDay={1}
                    headerToolbar={false}
                    height={630}
                    events={filteredEvents}
                    eventClick={onEventClickHandler}
                    editable={true}
                    eventChange={handleEventChange}
                />
            </div>
        ) : (
            <div>
                Loading calendar
            </div>
        )
    );
};

export default Calendar;