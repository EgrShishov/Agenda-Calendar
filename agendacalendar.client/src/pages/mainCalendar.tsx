import React, {useContext, useEffect, useState} from 'react';
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
import MeetingsSchedule from "../components/meetingsSchedule.tsx";
import {MeetingService} from "../services/meetingService.ts";
import {CalendarService} from "../services/calendarService.ts";
import {UserService} from "../services/userService.ts";
import {EventService} from "../services/eventService.ts";
import {ReminderService} from "../services/reminderService.ts";

const MainCalendar = () => {
    const {
        showEventDetails,
        showCalendarModal,
        showReminderModal,
        showWorkingHoursModal,
        showSuggestModal,
        setUsedColors,
        setShowSuggestModal,
        setShowWorkingHoursModal,
        setShowEventDetails,
        setShowCalendarModal,
        setShowReminderModal,
        setSharedCalendarsList,
        setCalendarsList
    } = useContext(GlobalContext);

    const [isSidebarOpen, setIsSidebarOpen] = useState(false);

    const toggleSidebar = () => {
        setIsSidebarOpen(!isSidebarOpen);
        setShowSuggestModal(false);
        setShowWorkingHoursModal(false);
        setShowEventDetails(false);
        setShowCalendarModal(false);
        setShowReminderModal(false);
    };

    const meetingService = new MeetingService();
    const calendarService = new CalendarService();
    const userService = new UserService();
    const eventService = new EventService();
    const reminderService = new ReminderService();

    const [meetings, setMeetings] = useState([]);

    useEffect(() => {
        const fetchMeetings = async () => {
            const response = await meetingService.getMeetings();
            if(response){
                setMeetings(response);
            }
        };
        fetchMeetings();
    }, [isSidebarOpen]);

    useEffect(() => {
        const fetchCalendars = async () => {
            try {
                const calendars = await calendarService.getCalendars();
                setCalendarsList(calendars.map((calendar) => {
                    return {
                        calendar,
                        checked: true
                    }
                }));

                const usedColors = calendars.map(calendar => calendar.calendarColor);
                setUsedColors(usedColors);
            } catch (error) {
                console.error('Error fetching calendars:', error);
            }
        };
        fetchCalendars();
    },[]);

    useEffect(() => {
        const fetchShared = async () =>
        {
            const calendars = await calendarService.getShared();
            if(calendars){
                setSharedCalendarsList(calendars.map((calendar) => {
                    return {
                        calendar,
                        checked: true
                    }
                }));
            }
        };
        fetchShared();
    });

   /* const meetings = [
        {
            id: 1,
            title: "Team Meeting",
            description: "Discuss project updates",
            startTime: "2024-05-26T12:00:00Z",
            endTime: "2024-05-26T13:00:00Z",
            participants: [{ name: "Alice" }, { name: "Bob" }],
            invitationStatus: "Accepted"
        },
        {
            id: 2,
            title: "Client Call",
            description: "Monthly sync with the client",
            startTime: "2024-05-26T14:00:00Z",
            endTime: "2024-05-26T15:00:00Z",
            participants: [{ name: "Alice" }, { name: "Charlie" }],
            invitationStatus: "Declined"
        },
        {
            "id": 3,
            "title": "Client Call",
            "description": "Monthly sync with the client.",
            "startTime": "2024-05-24T11:00:00Z",
            "endTime": "2024-05-24T12:00:00Z",
            "userId": 2,
            "participants": [
                {"id": 1, "name": "Alice"},
                {"id": 3, "name": "Charlie"}
            ],
            "isCancelled": true,
            "invitations": []
        },
        {
            "id": 4,
            "title": "Client Call",
            "description": "Monthly sync with the client.",
            "startTime": "2024-05-24T11:00:00Z",
            "endTime": "2024-05-24T12:00:00Z",
            "userId": 2,
            "participants": [
                {"id": 1, "name": "Alice"},
                {"id": 3, "name": "Charlie"}
            ],
            "isCancelled": true,
            "invitations": []
        },
        {
            "id": 5,
            "title": "Client Call",
            "description": "Monthly sync with the client.",
            "startTime": "2024-05-25T11:00:00Z",
            "endTime": "2024-05-25T12:00:00Z",
            "userId": 2,
            "participants": [
                {"id": 1, "name": "Alice"},
                {"id": 3, "name": "Charlie"}
            ],
            "isCancelled": true,
            "invitations": []
        },
        {
            "id": 5,
            "title": "Client Call",
            "description": "Monthly sync with the client.",
            "startTime": "2024-05-23T11:00:00Z",
            "endTime": "2024-05-23T12:00:00Z",
            "userId": 2,
            "participants": [
                {"id": 1, "name": "Alice"},
                {"id": 3, "name": "Charlie"}
            ],
            "isCancelled": true,
            "invitations": []
        },
        {
            "id": 5,
            "title": "Client Call",
            "description": "Monthly sync with the client.",
            "startTime": "2024-05-21T11:00:00Z",
            "endTime": "2024-05-21T12:00:00Z",
            "userId": 2,
            "participants": [
                {"id": 1, "name": "Alice"},
                {"id": 3, "name": "Charlie"}
            ],
            "isCancelled": true,
            "invitations": []
        }
    ]*/

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
                        <Calendar/>
                    </div>
                    <div className="col-span-1 items-center">
                        <UpcomingEvents/>
                    </div>

                    <div
                        className="absolute top-1/2 right-0 transform -translate-y-1/2 bg-orange-400 hover:bg-orange-500 text-white p-2 rounded-lg shadow-lg cursor-pointer z-50 transition duration-300 ease-in-out"
                        onClick={toggleSidebar}
                    >
                        {isSidebarOpen ? (
                            '>'
                        ) : (
                            '<'
                        )}
                    </div>
                    <div
                        className={`fixed top-0 right-0 h-full bg-white z-20 shadow-lg transform transition-transform ${
                            isSidebarOpen ? 'translate-x-0' : 'translate-x-full'
                        }`}
                        style={{width: '1200px'}}
                    >
                        <MeetingsSchedule meetings={meetings}/>
                    </div>

                </div>
            </div>

        </React.Fragment>
    );
}

export default MainCalendar;