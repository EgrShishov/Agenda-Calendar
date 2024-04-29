import CreateButton from "./createButton.tsx";
import UpcomingEvents from "./upcomingEvents.tsx";
import CalendarList from "./calendarList.tsx";

const Sidebar =() => {
    return (
        <aside className="border p-5 w-64">
            <CreateButton />
            <UpcomingEvents />
            <CalendarList />
        </aside>
    );
};

export default Sidebar;