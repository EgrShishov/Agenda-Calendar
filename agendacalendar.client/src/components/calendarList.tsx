import React, {useContext, useEffect, useState} from 'react';
import {CalendarService} from "../services/calendarService.ts";
import GlobalContext from "../context/globalContext.ts";

const labelsClasses = [
    "indigo",
    "gray",
    "green",
    "blue",
    "red",
    "purple",
];

const CalendarList = () =>{
    const {labels,
        updateLabel,
        calendarsList,
        setCalendarsList ,
        filteredEvents,
    } = useContext(GlobalContext);
    const calendarService = new CalendarService();

    const handleDownloadOnClick = async (e) => {
        e.preventDefault();
        const response = await calendarService.exportCalendar(e.target.value);
        console.log(response);
        const blob = new Blob([response.data], { type: 'text/calendar' });

        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', `calendar-${e.target.value}.ics`);
        link.click();
        window.URL.revokeObjectURL(url);
    };

    const handleDeleteOnClick = async (e) => {
        e.preventDefault();
        const calendarId = e.target.value;
        const response = await calendarService.deleteCalendar(calendarId);
        if(response){
            setCalendarsList(prev => prev.filter(calendar => calendar.calendar.id != calendarId));
        }
        console.log(response);
    }

    return (
        <React.Fragment>
            <p className="text-orange-400 font-bold text-xl mt-10">Calendars</p>
            <div className="mt-4">
                {calendarsList.map(({ calendar, checked }, idx) => (
                    <div key={idx} className="bg-gray-100 rounded-md p-2.5 mb-2 flex items-center">
                        <input
                            type="checkbox"
                            checked={checked}
                            onChange={() => updateLabel({ label: calendar.calendarColor, checked: !checked })}
                            className={`form-checkbox h-5 w-5 rounded focus:ring-0 cursor-pointer accent-${calendar.calendarColor}-400`}
                        />
                        <div className="flex-grow">
                            <p className="text-lg font-semibold text-gray-700">{calendar.title}</p>
                        </div>
                        <div className="flex ml-2">
                            <button onClick={handleDownloadOnClick} value={calendar.id} className="text-gray-600 hover:text-gray-800 focus:outline-none">
                                <i className="material-icons-outlined">download</i>
                            </button>
                            <button onClick={handleDeleteOnClick} value={calendar.id} className="text-gray-600 hover:text-gray-800 focus:outline-none ml-2">
                                <i className="material-icons-outlined">cancel</i>
                            </button>
                        </div>
                    </div>
                ))}
            </div>
        </React.Fragment>
    )
}

export default CalendarList;