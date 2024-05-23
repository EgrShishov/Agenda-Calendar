import {DatePickerCalendar} from "react-nice-dates";
import {enGB} from "date-fns/locale"
import {useEffect, useState} from "react";
import { format, addDays, startOfWeek, addWeeks } from 'date-fns';
import BookingForm from "./bookingForm.tsx";


const BookingCalendar = () => {
    const [scheduleTitle, setScheduleTitle] = useState('');
    const [scheduleDescription, setScheduleDescription] = useState('');
    const [scheduleOwnerName, setScheduleOwnerName] = useState('');

    const [selectedDate, setSelectedDate] = useState(new Date());
    const [availableSlots, setAvailableSlots] = useState([]);
    const [bookingDetails, setBookingDetails] = useState(null);
    const [weekStart, setWeekStart] = useState(startOfWeek(new Date(), { weekStartsOn: 1 }));

    useEffect(() => {
        const mockSlots = [
            { date: '2024-05-23', times: ['09:00 AM', '10:00 AM', '11:00 AM', '12:00 PM', '01:00 PM', '02:00 PM', '03:00 PM', '04:00 PM'] },
            { date: '2024-05-24', times: ['09:00 AM', '10:00 AM', '11:00 AM', '12:00 PM', '01:00 PM', '02:00 PM', '03:00 PM', '04:00 PM'] },
            { date: '2024-05-27', times: ['09:00 AM', '10:00 AM', '11:00 AM', '12:00 PM', '01:00 PM', '02:00 PM', '03:00 PM', '04:00 PM'] },
            { date: '2024-05-28', times: ['09:00 AM', '10:00 AM', '11:00 AM', '12:00 PM', '01:00 PM', '02:00 PM', '03:00 PM', '04:00 PM'] },
            { date: '2024-05-29', times: ['09:00 AM', '10:00 AM', '11:00 AM', '12:00 PM', '01:00 PM', '02:00 PM', '03:00 PM', '04:00 PM', '05:00 PM'] },
        ];

        const startDate = startOfWeek(selectedDate, { weekStartsOn: 1 });
        setWeekStart(startDate);

        const slots = [];
        for (let i = 0; i < 7; i++) {
            const date = addDays(startDate, i);
            const formattedDate = format(date, 'yyyy-MM-dd');
            const daySlots = mockSlots.find(slot => slot.date === formattedDate);
            slots.push({ date, times: daySlots ? daySlots.times : [] });
        }

        setAvailableSlots(slots);

        setScheduleDescription('test description');
        setScheduleOwnerName('Егор Шишов');
        setScheduleTitle('Agenda consultation');
    }, [selectedDate]);

    const handleSlotClick = (date, time) => {
        setBookingDetails({ date, time });
    };

    const handleBookingSubmit = (e) => {
        e.preventDefault();
        // Process booking details
        alert(`Booking confirmed for ${format(bookingDetails.date, 'dd MMM yyyy')} at ${bookingDetails.time}`);
        setBookingDetails(null);
    };

    const handlePrevWeek = () => {
        setSelectedDate(addWeeks(selectedDate, -1));
    };

    const handleNextWeek = () => {
        setSelectedDate(addWeeks(selectedDate, 1));
    };

    const handleSetToday = () => {
        setSelectedDate(new Date());
    };

    return (
        <div className="w-screen">
            <div className="m-4 border-2 border-gray-300 rounded-xl">
                <div className="m-4 flex items-center space-x-4">
                    <div className="flex items-center space-x-2">
                        <div className="border border-black/60 rounded-full w-16 h-16 flex items-center justify-center">
                            <span className="text-2xl font-bold">{scheduleOwnerName[0]}</span>
                        </div>
                        <span className="text-2xl font-bold text-black/60">
                            {scheduleOwnerName}
                        </span>
                    </div>

                    <span className="flex-1 text-xl font-normal text-black/60">
                        {scheduleTitle}
                    </span>
                    <span className="flex-1 text-xl font-normal text-black/60">
                        {scheduleDescription}
                    </span>
                    <span className="flex text-xl font-medium text-black/60">
                    <span className="text-orange-400">Agenda</span> Calendar
                </span>
                </div>
            </div>

            <div className="m-4 flex border-2 border-gray-300 rounded-xl">
                <div className="w-1/4 p-4 bg-transparent">
                    <span
                        className="m-2 text-xl font-normal text-black\60"
                    >
                        Select meeting time
                    </span>
                    <button
                        className="border rounded py-2 px-4 mr-5"
                        onClick={() => handleSetToday()}>
                        Today
                    </button>
                    <DatePickerCalendar
                        locale={enGB}
                        date={selectedDate}
                        onDateChange={setSelectedDate}
                    />
                </div>
                <div className="w-3/4 p-4">
                    <div className="flex justify-between items-center mb-4">
                        <button className="text-blue-500" onClick={handlePrevWeek}>
                        <span
                            className="material-icons-outlined text-3xl cursor-pointer text-gray-600 mx-2">
                                chevron_left
                        </span>
                        </button>
                        <h2 className="text-xl font-semibold">Available time for the week
                            of {format(weekStart, 'dd MMM yyyy')}</h2>
                        <button className="text-blue-500" onClick={handleNextWeek}>
                         <span className="material-icons-outlined text-3xl cursor-pointer text-gray-600 mx-2">
                                chevron_right
                        </span>
                        </button>
                    </div>
                    <div className="grid grid-cols-7 gap-4 mb-4">
                        {availableSlots.map((slot, index) => (
                            <div key={index} className="border p-2 rounded">
                                <h3 className="text-md font-semibold mb-2">{format(slot.date, 'E dd MMM')}</h3>
                                {slot.times.length > 0 ? (
                                    slot.times.map((time, idx) => (
                                        <button
                                            key={idx}
                                            className="block w-full px-2 py-2 border border-orange-500
                                            bg-white-500 hover:bg-gray-100 hover:scale-105 transition ease-out duration-200 transform
                                            text-sm text-orange-400 rounded mb-1"
                                            onClick={() => handleSlotClick(slot.date, time)}
                                        >
                                            {time}
                                        </button>
                                    ))
                                ) : (
                                    <p className="text-center text-gray-500">No slots</p>
                                )}
                            </div>
                        ))}
                    </div>
                    {bookingDetails && (
                        <div className="fixed inset-0 flex items-center justify-center bg-gray-900 bg-opacity-50">
                            <div className="bg-white p-6 rounded shadow-md w-1/3">
                                <h3 className="text-lg font-semibold mb-4">Book Appointment</h3>
                                <BookingForm
                                    bookingDetails={bookingDetails}
                                    setBookingDetails={setBookingDetails}
                                />
                            </div>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default BookingCalendar;