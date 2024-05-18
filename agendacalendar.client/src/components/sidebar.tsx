import CreateButton from "./createButton.tsx";
import UpcomingEvents from "./upcomingEvents.tsx";
import CalendarList from "./calendarList.tsx";

const Sidebar =() => {
    return (
        <div className="border border-gray-300 rounded p-3 h-screen overflow-y-auto flex flex-col">
            <div className="mt-4 flex justify-center">
                <CreateButton/>
            </div>
            <CalendarList/>
        </div>
    );
};

export default Sidebar;