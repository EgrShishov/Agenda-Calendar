import CreateButton from "./createButton.tsx";
import UpcomingEvents from "./upcomingEvents.tsx";
import CalendarList from "./calendarList.tsx";
import SubscribedCalendarList from "./subscribedCalendarList.tsx";

const Sidebar = () => {
    return (
        <div className="border border-gray-300 rounded p-3 max-h-[650px] flex flex-col">
            <div className="flex justify-center">
                <CreateButton/>
            </div>
            <div className="mt-4 flex-1 overflow-y-auto">
                <div className="flex flex-col gap-y-2.5">
                    <CalendarList/>
                    <SubscribedCalendarList/>
                </div>
            </div>
        </div>
    );
};


export default Sidebar;