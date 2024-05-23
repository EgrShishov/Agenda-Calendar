import {addDays, format, startOfWeek} from "date-fns";

const MeetingsSchedule = ({meetings}) => {

    const renderMeetings = (day) => {
        return meetings
            .filter(meeting => format(new Date(meeting.startTime), 'yyyy-MM-dd') === format(day, 'yyyy-MM-dd'))
            .map(meeting => (
                <div key={meeting.id} className="bg-blue-200 p-2 rounded mb-2">
                    <h3>{meeting.title}</h3>
                    <p>{format(new Date(meeting.startTime), 'HH:mm')} - {format(new Date(meeting.endTime), 'HH:mm')}</p>
                </div>
            ));
    };

    const renderDays = () => {
        const days = [];
        const start = startOfWeek(new Date());

        for (let i = 0; i < 7; i++) {
            const day = addDays(start, i);
            days.push(
                <div key={i} className="w-1/7 p-4">
                    <h2 className="text-xl text-gray-400 font-bold">
                        {format(day, 'EEEE')}
                    </h2>
                    {renderMeetings(day)}
                </div>
            );
        }
        return days;
    };

    return (
        <div className="flex justify-between">
            {renderDays()}
        </div>
    );
}

export default MeetingsSchedule;