import React, {useEffect, useState} from 'react';
import {CalendarService} from "../services/calendarService.ts";

const CalendarList = () =>{
    const [calendarsList, setCalendarsList] = useState([]);
    const calendarService = new CalendarService();

    useEffect(() => {
        const fetchCalendars = async () => {
            try {
                const userId = 13;
                const calendars = await calendarService.getCalendars(userId);
                setCalendarsList(calendars);
            } catch (error) {
                console.error('Error fetching calendars:', error);
            }
        };
        fetchCalendars();
    }, [calendarService]);

    return (
        <React.Fragment>
            <p className="text-orange-400 font-bold mt-10">Calendars</p>
            <div className="text-orange-400 font-bold flex items-center justify-center">
                <ul>
                    {calendarsList.map(calendar => (
                        <li key={calendar.id}>{calendar.title}</li>
                    ))}
                </ul>
            </div>
        </React.Fragment>
    )
}

export default CalendarList;