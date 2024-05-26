import {EventService} from "../services/eventService.ts";
import {useEffect, useState, useContext} from "react";
import GlobalContext from "../context/globalContext.ts";
import MeetingsSchedule from "./meetingsSchedule.tsx";

const   UpcomingEvents = () => {

    const eventService = new EventService();
    const {events} = useContext(GlobalContext);
    const [upcomingEvents, setUpcomingEvents] = useState([]);

    useEffect(() => {
        const fetchUpcomingEvents = async() =>{
            try{
                const events = await eventService.getUpcoming();
                setUpcomingEvents(events);
                console.log(events);
            }
            catch(error){
                console.error(error);
            }
        };

        fetchUpcomingEvents();
    });

    const formatDate = (dateTimeString) => {
        const options = {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            hour: 'numeric',
            minute: 'numeric',
            second: 'numeric',
        };

        const formattedDate = new Date(dateTimeString).toLocaleDateString(undefined, options);
        return formattedDate;
    };

    return (
        <div className="border border-gray-300 rounded p-3 h-screen overflow-y-auto max-h-[650px]">
            <div className="border border-gray-300 rounded p-2">
                <p className="text-lg font-bold mb-2">Upcoming events</p>
                {upcomingEvents.length > 0 ? (
                    <div>
                        {upcomingEvents.slice(0, 5).map((event, index) => (
                            <div key={index} className="flex items-center mb-2">
                                <p className="text-l font-semibold text-gray-700 truncate"
                                   style={{maxWidth: '200px'}}>{event.title} -
                                    <p className="text-ms font-light">{formatDate(event.startTime)}</p>
                                </p>
                            </div>
                        ))}
                    </div>
                ) : (
                    <p>No upcoming events</p>
                )}
            </div>
        </div>
    )
}

export default UpcomingEvents;