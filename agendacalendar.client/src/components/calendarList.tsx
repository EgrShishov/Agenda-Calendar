import React, {useContext, useState} from 'react';
import {CalendarService} from "../services/calendarService.ts";
import GlobalContext from "../context/globalContext.ts";
import {MenuItem, Menu} from "@mui/material";

const CalendarList = () =>{
    const {
        labelsClasses,
        labels,
        updateLabel,
        calendarsList,
        setCalendarsList ,
        filteredEvents,
    } = useContext(GlobalContext);

    const calendarService = new CalendarService();

    const [isCollapsed, setIsCollapsed] = useState(false);
    const [selectedMenuIndex, setSelectedMenuIndex] = useState(null);

    const toggleCollapse = () => {
        setIsCollapsed(!isCollapsed);
        setSelectedMenuIndex(null);
    };

    const handleActionClick = (action, calendarId) => {
        console.log('Performing action:', action, 'on calendar:', calendarId);
    };

    const [anchorEl, setAnchorEl] = useState(null);

    const handleMenuToggle = (event) => {
        setAnchorEl(event.currentTarget);
    };

    const handleMenuClose = () => {
        setAnchorEl(null);
    };

    const handleDownloadOnClick = async (calendarId) => {
        const response = await calendarService.exportCalendar(calendarId);
        console.log(calendarId, response);
        const blob = new Blob([response], { type: 'text/calendar' });

        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', `calendar-${calendarId}.ics`);
        link.click();
        window.URL.revokeObjectURL(url);

        handleMenuClose();
    };

    const handleDeleteOnClick = async (calendarId) => {
        const response = await calendarService.deleteCalendar(calendarId);
        if(response){
            setCalendarsList(prev => prev.filter(calendar => calendar.calendar.id != calendarId));
        }

        handleMenuClose();
    }

    const handleEditOnClick = async(calendarId) => {
        handleMenuClose();
    };

    return (
        <React.Fragment>
            <div className="flex justify-between items-center mt-4">
                <button onClick={toggleCollapse} className="text-gray-600 hover:text-gray-800 focus:outline-none">
                    <div className="flex items-end">
                        <p className="text-gray-400 font-bold text-xl mt-10">Your calendars: </p>
                        {

                            isCollapsed ?
                                <span className="material-icons-outlined cursor-pointer text-gray-600 mx-2">
                                keyboard_arrow_down
                            </span>
                                :
                                <span className="material-icons-outlined cursor-pointer text-gray-600 mx-2">
                                keyboard_arrow_up
                            </span>
                        }
                    </div>
                </button>
            </div>
            <div className="mt-4">
                {!isCollapsed && calendarsList.map(({calendar, checked}, idx) => (
                    <div key={idx} className="bg-gray-100 rounded-md p-2.5 mb-2 flex items-center relative">
                        <input
                            type="checkbox"
                            checked={checked}
                            onChange={() => updateLabel({label: calendar.calendarColor, checked: !checked})}
                            className={`form-checkbox h-5 w-5 rounded focus:ring-0 cursor-pointer`}
                            style={{accentColor: calendar.calendarColor}}
                        />
                        <div className="flex-grow ml-3">
                            <p className="text-l font-semibold text-gray-700">{calendar.title}</p>
                        </div>
                        <div className="relative ml-3">
                            <button onClick={handleMenuToggle} className="text-gray-600 focus:outline-none">
                                <span className="material-icons-outlined">more_vert</span>
                            </button>
                            <Menu
                                anchorEl={anchorEl}
                                keepMounted
                                open={Boolean(anchorEl)}
                                onClose={handleMenuClose}
                                anchorOrigin={{
                                    vertical: 'top',
                                    horizontal: 'right'
                                }}
                                transformOrigin={{
                                    vertical: 'top',
                                    horizontal: 'left',
                                }}
                            >
                                <MenuItem onClick={() => handleDownloadOnClick(calendar.id)}>
                                    Download
                                </MenuItem>
                                <MenuItem onClick={() => handleDeleteOnClick(calendar.id)}>
                                    Delete
                                </MenuItem>
                                <MenuItem onClick={() => handleEditOnClick(calendar.id)}>
                                    Edit
                                </MenuItem>
                            </Menu>
                        </div>
                    </div>
                ))}
            </div>
        </React.Fragment>
    )
}

export default CalendarList;