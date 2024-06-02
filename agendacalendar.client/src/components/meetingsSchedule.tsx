import {addDays, endOfWeek, format, startOfWeek} from "date-fns";
import GlobalContext from "../context/globalContext.ts";
import {useContext} from "react";
import {MeetingService} from "../services/meetingService.ts";

const getStatusColor = (status) => {
    switch (status) {
        case 0:
            return 'bg-yellow-100 border-l-4 border-yellow-500';
        case 1:
            return 'bg-green-100 border-l-4 border-green-500';
        case 2:
            return 'bg-red-100 border-l-4 border-red-500';
        default:
            return 'bg-blue-100 border-l-4 border-blue-500';
    }
};

const MeetingsSchedule = ({ meetings }) => {
    const {setShowSuggestModal} = useContext(GlobalContext);

    console.log(meetings);
    const meetingService = new MeetingService();

    const handleAccept = async (meetingId) => {
        const response = await meetingService.acceptMeeting(meetingId);
        console.log(response);
    };

    const handleDecline = async (meetingId) => {
        const response = await meetingService.declineMeeting(meetingId);
        console.log(response);
    };

    const renderMeetings = (day) => {
        return meetings
            .filter(meeting => format(new Date(meeting.startTime), 'yyyy-MM-dd') === format(day, 'yyyy-MM-dd'))
            .map(meeting => (
                <div key={meeting.id}
                     className={`p-2 rounded mb-3 shadow-lg ${getStatusColor(meeting.invitationStatus)}`}>
                    <h3 className="text-lg font-semibold text-gray-700 break-words">{meeting.title}</h3>
                    <p className="text-sm text-gray-600 break-words">{meeting.description}</p>
                    <p className="text-sm text-gray-500">{format(new Date(meeting.startTime), 'HH:mm')} - {format(new Date(meeting.endTime), 'HH:mm')}</p>
                   {/* <p className="text-sm text-gray-600 break-words">Participants: {meeting.participants.map(p => p.name).join(', ')}</p>
                    {meeting.invitationStatus === 'Declined' && <p className="text-red-500 font-bold">Cancelled</p>}*/}
                    {meeting.invitationStatus === 2 && <p className="text-red-500 font-bold">Cancelled</p>}
                    {meeting.invitationStatus === 0 && (
                        <div className="mt-2 flex-1 gap-y-2">
                            <button
                                onClick={() => handleAccept(meeting.id)}
                                className="px-4 py-2 mr-2 text-white bg-green-500 rounded hover:bg-green-700">
                                Подтвердить
                            </button>
                            <button
                                onClick={() => handleDecline(meeting.id)}
                                className="px-4 py-2 text-white bg-red-500 rounded hover:bg-red-700">
                                Отклонить
                            </button>
                        </div>
                    )}
                </div>
            ));
    };

    const renderDays = () => {
        const days = [];
        const start = startOfWeek(new Date());
        const end = endOfWeek(new Date());

        for (let i = 0; i < 7; i++) {
            const day = addDays(start, i);
            days.push(
                <div key={i} className="flex-1 p-2">
                    <h2 className="text-xl text-gray-600 font-bold mb-2">
                        {format(day, 'EEEE')}
                    </h2>
                    {renderMeetings(day)}
                </div>
            );
        }
        return days;
    };

    const handleNewMeeting = () => {
        setShowSuggestModal(true);
    }

    return (
        <div className="flex flex-col items-center z-20 p-3 bg-gray-100 min-h-screen">
            <div className="m-2">
                <button
                    className="border bg-white rounded-lg py-2 px-4 mr-5 hover:scale-105 transition ease-out"
                    onClick={handleNewMeeting}>
                    Submit meeting
                </button>
                <span className="text-xl font-bold text-gray-800 mb-8">
                Meetings Schedule for {format(startOfWeek(new Date()), 'MMM d')} - {format(endOfWeek(new Date()), 'MMM d')}
            </span>
            </div>
            <div className="flex justify-between overflow-y-auto w-full h-screen bg-white shadow-md rounded-lg p-6">
                {renderDays()}
            </div>
        </div>
    );
};

export default MeetingsSchedule;