import React, {useContext, useRef} from 'react';
import CalendarHeader from "../components/calendarHeader.tsx";
import Sidebar from "../components/sidebar.tsx";
import Calendar from "../components/calendar.tsx";
import EventDetails from "../components/eventDetails.tsx";
import GlobalContext from "../context/globalContext.ts";
import CalendarModal from "../components/calendarModal.tsx";
import ReminderModal from "../components/reminderModal.tsx";
import UpcomingEvents from "../components/upcomingEvents.tsx";
import calendarHeader from "../components/calendarHeader.tsx";

const MainCalendar = () => {
    const {showEventDetails, showCalendarModal, showReminderModal} = useContext(GlobalContext);

    return (
        <React.Fragment>
            {showEventDetails && <EventDetails/>}
            {showCalendarModal && <CalendarModal/>}
            {showReminderModal && <ReminderModal/>}

            <div className="h-screen w-full flex flex-col">
                <CalendarHeader />
                <div className="flex-1 grid grid-cols-6">
                    <div className="col-span-1 item-center">
                        <Sidebar/>
                    </div>
                    <div className="col-span-4 items-center">
                        <Calendar />
                    </div>
                    <div className="col-span-1 items-center">
                        <UpcomingEvents/>
                    </div>
                </div>
            </div>
        </React.Fragment>
    );
}

export default MainCalendar;