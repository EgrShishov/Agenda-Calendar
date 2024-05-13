import CreateButton from "./createButton.tsx";
import UpcomingEvents from "./upcomingEvents.tsx";
import CalendarList from "./calendarList.tsx";

const Sidebar =() => {
    return (
        <div className="border p-3 flex-1">
            <CreateButton />
            <CalendarList />
        </div>
    );
};

export default Sidebar;