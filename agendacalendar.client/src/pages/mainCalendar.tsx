import React, {useContext} from 'react';
import CalendarHeader from "../components/calendarHeader.tsx";
import Sidebar from "../components/sidebar.tsx";
import Calendar from "../components/calendar.tsx";
import EventDetails from "../components/eventDetails.tsx";
import GlobalContext from "../context/globalContext.ts";

const MainCalendar = () => {
    const {showEventDetails} = useContext(GlobalContext);

    return (
        <React.Fragment>
            {showEventDetails && <EventDetails />}
            <div className="w-full h-screen grid-cols-2 grid-rows-1">
                <div>
                    <CalendarHeader/>
                </div>
                <div className="flex flex-2">
                    <Sidebar/>
                    <Calendar/>
                </div>
            </div>
        </React.Fragment>
    );
};

export default MainCalendar;