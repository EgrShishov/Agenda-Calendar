import React, {useContext} from 'react';
import CalendarHeader from "../components/calendarHeader.tsx";
import Sidebar from "../components/sidebar.tsx";
import Calendar from "../components/calendar.tsx";
import EventDetails from "../components/eventDetails.tsx";
import GlobalContext from "../context/globalContext.ts";
import CalendarModal from "../components/calendarModal.tsx";
import ReminderModal from "../components/reminderModal.tsx";
import UpcomingEvents from "../components/upcomingEvents.tsx";
import WorkingHoursForm from "../components/workingHoursForm.tsx";
import SuggestModal from "../components/suggestModal.tsx";

const MainCalendar = () => {
    const {
        showEventDetails,
        showCalendarModal,
        showReminderModal,
        showWorkingHoursModal,
        showSuggestModal
    } = useContext(GlobalContext);

    return (
        <React.Fragment>
            {showEventDetails && <EventDetails/>}
            {showCalendarModal && <CalendarModal/>}
            {showReminderModal && <ReminderModal/>}
            {showWorkingHoursModal && <WorkingHoursForm/>}
            {showSuggestModal && <SuggestModal/>}

            <div className="h-screen w-full flex flex-col">
                <CalendarHeader />
                <div className=" h-screen grid grid-cols-6">
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