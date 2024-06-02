import {DatePickerCalendar} from "react-nice-dates";
import {enGB} from "date-fns/locale"
import {useEffect, useState} from "react";
import { format, addDays, startOfWeek, addWeeks } from 'date-fns';
import BookingForm from "./bookingForm.tsx";
import {useNavigate} from "react-router-dom";
import {BookingService} from "../services/bookingService.ts";
import React from "react";


const BookingCalendar = ({email}) => {

    const [scheduleTitle, setScheduleTitle] = useState('');
    const [scheduleDescription, setScheduleDescription] = useState('');
    const [scheduleOwnerName, setScheduleOwnerName] = useState('');

    const [selectedDate, setSelectedDate] = useState(new Date());
    const [availableSlots, setAvailableSlots] = useState([]);
    const [bookingDetails, setBookingDetails] = useState(null);
    const [weekStart, setWeekStart] = useState(startOfWeek(new Date(), { weekStartsOn: 1 }));

    const [message, setMessage] = useState('');
    const Redirect = useNavigate();

    const bookingService = new BookingService();

    const [mockSlots, setMockSlots] = useState([]);
    useEffect(() => {
        const getSchedule = async () =>{
            const response = await bookingService.getAvaibaleSlots(email);

            if(response) {
                setMockSlots(response.slots);
                setScheduleDescription(response.description);
                setScheduleOwnerName(response.userName);
                setScheduleTitle(response.title);
            } else {
                setMessage(response);
            }
        };
        getSchedule();
    },[]);

    useEffect(() => {
        const startDate = startOfWeek(selectedDate, { weekStartsOn: 1 });
        setWeekStart(startDate);

        const slotsByDate = {};
        mockSlots.forEach(slot => {
            const dateKey = format(new Date(slot.date), 'yyyy-MM-dd');
            if (!slotsByDate[dateKey]) {
                slotsByDate[dateKey] = { date: new Date(slot.date), times: [], isBooked: slot.isBooked };
            }
            slot.times.forEach(time => {
                slotsByDate[dateKey].times.push({ time, slotId: slot.id, isBooked: slot.isBooked });
            });
        });

        const slotsArray = [];
        for (let i = 0; i < 7; i++) {
            const date = addDays(startDate, i);
            const formattedDate = format(date, 'yyyy-MM-dd');
            slotsArray.push({
                date,
                times: slotsByDate[formattedDate] ? slotsByDate[formattedDate].times : []
            });
        }

        setAvailableSlots(slotsArray);
        console.log(slotsArray);

    }, [selectedDate, mockSlots]);

    const handleSlotClick = (date, time, slotId) => {
        setBookingDetails({ date, time, slotId });
    };

    const handleBookingSubmit = (e) => {
        e.preventDefault();
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
        <React.Fragment>
            {scheduleOwnerName != '' ? (
                <div className="w-screen">
                    <div className="m-4 border-2 border-gray-300 rounded-xl">
                        <div className="m-4 flex items-center space-x-4">
                            <div className="flex items-center space-x-2">
                                <div
                                    className="border border-black/60 rounded-full w-16 h-16 flex items-center justify-center">
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
                            <span
                                className="flex text-xl font-medium text-black/60
                        hover:scale-105 transition ease-out hover:text-black/60 hover:font-bold"
                            >
                        <a href="http://localhost:5173/u">
                            <span className="text-orange-400">Agenda</span> Calendar
                        </a>
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
                                            slot.times.map((timeObj, idx) => (
                                                <button
                                                    key={idx}
                                                    className={`block w-full px-2 py-2 border ${
                                                        timeObj.isBooked ? 'bg-gray-300 text-gray-500 cursor-not-allowed' : 'border-orange-500 bg-white-500 hover:bg-gray-100 hover:scale-105 transition ease-out duration-200 transform text-sm text-orange-400'
                                                    } rounded mb-1`}
                                                    onClick={() => !timeObj.isBooked && handleSlotClick(slot.date, timeObj.time, timeObj.slotId)}
                                                    disabled={timeObj.isBooked}
                                                >
                                                    {timeObj.time}
                                                </button>
                                            ))
                                        ) : (
                                            <p className="text-center text-gray-500">No slots</p>
                                        )}
                                    </div>
                                ))}
                            </div>

                            {bookingDetails && (
                                <div
                                    className="fixed inset-0 flex items-center justify-center bg-gray-900 bg-opacity-50">
                                    <div className="bg-white p-6 rounded shadow-md w-1/3">
                                        <BookingForm
                                            scheduleDescription={scheduleDescription}
                                            scheduleTitle={scheduleTitle}
                                            bookingDetails={bookingDetails}
                                            setBookingDetails={setBookingDetails}
                                        />
                                    </div>
                                </div>
                            )}
                        </div>
                    </div>
                </div>
            ) : (
                <div className="w-screen flex items-center justify-center h-screen bg-gray-100">
                    <div
                        className="m-4 p-8 border-2 border-gray-300 rounded-xl bg-white shadow-lg flex flex-col items-center">
                        <svg className="w-16 h-16 text-gray-400 mb-4" fill="currentColor" viewBox="0 0 24 24"
                             xmlns="http://www.w3.org/2000/svg">
                            <path d="M12 2a10 10 0 100 20 10 10 0 000-20zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/>
                        </svg>
                        <span className="text-2xl font-bold text-gray-600 text-center mb-4">
                             User has not created a schedule of meetings or it is not yet available
                        </span>
                        <a href="/u"
                           className="px-4 py-2 bg-orange-500 text-white rounded-full shadow hover:bg-gray-300 focus:outline-none focus:ring-2 focus:ring-orange-500 focus:ring-opacity-50">
                            Go back
                        </a>
                    </div>
                </div>
            )}
        </React.Fragment>
    );
};

export default BookingCalendar;