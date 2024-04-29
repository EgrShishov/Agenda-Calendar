import CalendarHeader from "../components/calendarHeader.tsx";
import Sidebar from "../components/sidebar.tsx";
import Calendar from "../components/calendar.tsx";

const MainCalendar = () => {
    return (
        <div className="h-screen grid-cols-2 grid-rows-1">
            <div>
                <CalendarHeader/>
            </div>
            <div className="flex flex-2">
                <Sidebar/>
                <Calendar/>
            </div>
        </div>
    );
};

export default MainCalendar;