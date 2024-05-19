import React, {useContext, useState} from "react";
import {CalendarService} from "../services/calendarService.ts";
import GlobalContext from "../context/globalContext.ts";

const SubscribedCalendarList = () => {
    const {
        updateLabel,
        sharedCalendarsList,
        setSharedCalendarsList
    } = useContext(GlobalContext);

    const calendarService = new CalendarService();

    const [isCollapsed, setIsCollapsed] = useState(false);
    const toggleCollapse = () => {
        setIsCollapsed(!isCollapsed);
    };

    const handleUnsubscribeOnClick = async (calendarId) => {
        const response = await calendarService.unsubscribeFromCalendar(calendarId);
        if(response){
            setSharedCalendarsList(prev => prev.filter(calendar => calendar.calendar.id != calendarId));
        }
    };

    return (
        sharedCalendarsList ? (
            <React.Fragment>
                <div className="border border-gray-300 rounded p-1">
                    <div className="flex justify-between items-center p-2">
                        <button onClick={toggleCollapse}
                                className="text-gray-600 hover:text-gray-800 focus:outline-none">
                            <div className="flex items-center">
                                <p className="text-lg font-bold mb-2">Shared calendars: </p>
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
                    <div className="h-full">
                        {!isCollapsed && sharedCalendarsList.map(({calendar, checked}, idx) => (
                            <div key={idx} className="bg-gray-100 rounded-md p-2.5 mb-2 flex items-center">
                                <input
                                    type="checkbox"
                                    checked={checked}
                                    onChange={() => updateLabel({label: calendar.calendarColor, checked: !checked})}
                                    className={`form-checkbox h-5 w-5 rounded focus:ring-0 cursor-pointer`}
                                    style={{accentColor: calendar.calendarColor}}
                                />
                                <div className="flex-grow ml-3 overflow-hidden">
                                    <p className="text-l font-semibold text-gray-700 truncate"
                                       style={{maxWidth: '200px'}}>
                                        {calendar.title}
                                    </p>
                                </div>
                                <div className="relative ml-3 flex-shrink-0">
                                    <button
                                        className="text-ms font-semibold text-black\60"
                                        onClick={() => handleUnsubscribeOnClick(calendar.id)}
                                    >
                                        <span className="material-icons-outlined cursor-pointer text-gray-600 mx-2">
                                            close
                                        </span>
                                    </button>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </React.Fragment>
        ) : (
            <div>
                Loading calendars
            </div>
        )
    );
};

export default SubscribedCalendarList;