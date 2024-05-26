import dayjs from 'dayjs';
import {useContext, useState} from "react";
import TextField from '@mui/material/TextField';
import { TimePicker } from '@mui/x-date-pickers/TimePicker';
import {LocalizationProvider} from "@mui/x-date-pickers";
import {AdapterDayjs} from "@mui/x-date-pickers/AdapterDayjs";
import {MeetingService} from "../services/meetingService.ts";
import GlobalContext from "../context/globalContext.ts";

const WorkingHoursForm = () => {
    const daysOfWeek = ['Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa', 'Su'];

    const convertToIndex = (day) => {
        return daysOfWeek.indexOf(day) ? daysOfWeek.indexOf(day) : -1;
    };

    const {setShowWorkingHoursModal} = useContext(GlobalContext);

    const meetingService = new MeetingService();

    const [noFreeTime, setNoFreeTime] = useState({
        Mo: false,
        Tu: false,
        We: false,
        Th: false,
        Fr: false,
        Sa: false,
        Su: false
    });

    const [periods, setPeriods] = useState({
        Mo: { startTime: '09:00', endTime: '17:00' },
        Tu: { startTime: '09:00', endTime: '17:00' },
        We: { startTime: '09:00', endTime: '17:00' },
        Th: { startTime: '09:00', endTime: '17:00' },
        Fr: { startTime: '09:00', endTime: '17:00' },
        Sa: { startTime: '09:00', endTime: '17:00' },
        Su: { startTime: '09:00', endTime: '17:00' }
    });

    const [workingHours, setWorkingHours] = useState({
        Mo: [{ startTime: '09:00', endTime: '17:00' }],
        Tu: [{ startTime: '09:00', endTime: '17:00' }],
        We: [{ startTime: '09:00', endTime: '17:00' }],
        Th: [{ startTime: '09:00', endTime: '17:00' }],
        Fr: [{ startTime: '09:00', endTime: '17:00' }],
        Sa: [{ startTime: '09:00', endTime: '17:00' }],
        Su: [{ startTime: '09:00', endTime: '17:00' }]
    });

    const handleTimeChange = (day, periodIndex, key, value) => {
        setWorkingHours((prev) => ({
            ...prev,
            [day]: prev[day].map((period, index) => {
                if (index === periodIndex) {
                    return {
                        ...period,
                        [key]: value.format('HH:mm')
                    };
                }
                return period;
            })
        }));
    };

    const handleNoFreeTimeClick = (day, index) => {
        setWorkingHours((prev) => ({
            ...prev,
            [day]: prev[day].filter((_, i) => i !== index)
        }));
    };

    const handleAddNewPeriodClick = (day) => {
        setWorkingHours((prev) => ({
            ...prev,
            [day]: [...prev[day], { startTime: '09:00', endTime: '17:00', noFreeTime: false }]
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const dailyHours = Object.entries(workingHours).flatMap(([day, periods]) =>
            periods.map(period => ({
                day: convertToIndex(day) == -1 ? 0 : convertToIndex(day),
                startTime: period.startTime ? new Date(`1970-01-01T${period.startTime}:00`).toISOString() : '',
                endTime: period.endTime ? new Date(`1970-01-01T${period.endTime}:00`).toISOString() : ''
            }))
        );

        const request = {
            day: 0,
            dailyHours: dailyHours
        };
        console.log(request);
        const response = await meetingService.setWorkingHours(request);
        if(response.status === 200){
            setShowWorkingHoursModal(false);
        } else {
            console.error(response.data);g
        }
    };

    return (
        <div className="my-2 w-full fixed h-screen flex justify-center items-center rounded-xl z-40">
            <LocalizationProvider dateAdapter={AdapterDayjs}>
                <form onSubmit={handleSubmit}
                      className="flex overflow-y-auto h-screen flex-col space-y-3 p-4 bg-white m-3 fixed w-4/12
                       rounded-xl shadow-2xl">
                    <header className="rounded-t-xl flex justify-between items-center justify-content-end">
                        <div className="">
                            <button
                                onClick={() => setShowWorkingHoursModal(false)}
                            >
                              <span className="mx-2 material-icons-outlined font-light text-black-65">
                                  close
                              </span>
                            </button>
                        </div>
                    </header>

                    <h2 className="text-xl font-bold mb-3">Set Your Working Hours</h2>
                    <div className="space-y-3">
                        {Object.entries(workingHours).map(([day, periods], index) => (
                            <div key={day}>
                                <div className="flex items-center space-x-4">
                                    <span className="w-16">{day}</span>
                                    <div className="flex flex-col gap-y-3">
                                        {periods.map((period, periodIndex) => (
                                            <div key={periodIndex} className="flex items-center space-x-4">
                                                <TimePicker
                                                    label="Start Time"
                                                    value={dayjs(`1970-01-01T${period.startTime}:00`)}
                                                    onChange={(value) => handleTimeChange(day, periodIndex, 'startTime', value)}
                                                    renderInput={(params) => <TextField {...params} />}
                                                />
                                                <TimePicker
                                                    label="End Time"
                                                    value={dayjs(`1970-01-01T${period.endTime}:00`)}
                                                    onChange={(value) => handleTimeChange(day, periodIndex, 'endTime', value)}
                                                    renderInput={(params) => <TextField {...params} />}
                                                />
                                                {periodIndex === 0 && (
                                                    <button onClick={() => handleAddNewPeriodClick(day)}>
                                                        <span className="material-icons-outlined text-black/60 text-xl">add_circle_outline</span>
                                                    </button>
                                                )}
                                                <button onClick={() => handleNoFreeTimeClick(day, periodIndex)}>
                                                    <span className="material-icons-outlined text-black/60 text-xl">do_not_disturb</span>
                                                </button>
                                            </div>
                                        ))}
                                        {noFreeTime[day] && (
                                            <span>Нельзя бронировать</span>
                                        )}
                                    </div>
                                </div>
                            </div>
                        ))}

                    </div>

                    <button
                        type="submit"
                        className="hover:bg-black/60 bg-orange-400 px-6 py-2 rounded text-black hover:text-white
                            hover:scale-105 transition ease-out duration-200 transform"
                    >
                        Next
                    </button>

                </form>
            </LocalizationProvider>
        </div>
    );
};

export default WorkingHoursForm;