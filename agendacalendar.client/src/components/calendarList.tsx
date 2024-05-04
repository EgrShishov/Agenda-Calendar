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
    const {labels, updateLabel, calendarsList, setCalendarsList } = useContext(GlobalContext);
    const calendarService = new CalendarService();

    const handleDownloadOnClick = async (e) => {
        e.preventDefault();
        const response = await calendarService.exportCalendar(e.target.value);
        console.log(response);
        // const blob = new Blob([response.data], { type: 'text/calendar' });
        //
        // const url = window.URL.createObjectURL(blob);
        // const link = document.createElement('a');
        // link.href = url;
        // link.setAttribute('download', `cale.ics`); // Set the filename here
        // link.click();
        // window.URL.revokeObjectURL(url);
    };

    const handleDeleteOnClick = async (e) => {
        e.preventDefault();
        const calendarId = e.target.value;
        const response = await calendarService.deleteCalendar(calendarId);
        if(response){
            setCalendarsList(prev => prev.filter(calendar => calendar.id != calendarId));
        }
        console.log(response);
    }

    return (
        <React.Fragment>
            <p className="text-orange-400 font-bold mt-10">Calendars</p>
            <div className="text-orange-400 font-bold flex items-center justify-center">
                <ul>
                    {calendarsList.map(({ calendar: calendar, checked }, idx) => (
                        <li>
                            <label key={idx} className="items-center mt-3 block">
                                <input
                                    type="checkbox"
                                    checked={checked}
                                    onChange={() =>
                                        updateLabel({label: calendar, checked: !checked})
                                    }
                                    className={`form-checkbox h-5 w-5 text-${calendarsList[idx].title}-400 rounded focus:ring-0 cursor-pointer`}
                                />
                                <span
                                    className="text-l text-gray-400 font-semibold mx-2">{calendarsList[idx].title} </span>
                                <button
                                    onClick={handleDownloadOnClick}
                                    value={calendarsList[idx].id}
                                    className="material-icons-outlined font-light text-black-65 cursor-pointer">
                                    download
                                </button>
                                <button
                                    onClick={handleDeleteOnClick}
                                    value={calendarsList[idx].id}
                                    className="material-icons-outlined font-light text-black-65 cursor-pointer">
                                    cancel
                                </button>
                            </label>
                        </li>
                    ))}
                </ul>
            </div>
        </React.Fragment>
    )
}

export default CalendarList;