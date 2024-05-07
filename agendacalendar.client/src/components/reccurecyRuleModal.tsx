import {useState} from "react";
import {
    TextField,
    Select,
    MenuItem,
    InputLabel,
    FormControl,
    Button,
} from '@mui/material';
import {ReccurencyDate, ReccurencyRule} from "../models/reccurencyRulePatternModel.ts";

const ReccurecyRuleModal = ({setShowReccurenceModal}) => {
    const [interval, setInterval] = useState(1);
    const [frequency, setFrequency] = useState(0);
    const [daysOfWeek, setDaysOfWeek] = useState([0]);
    const [daysOfMonth, setDaysOfMonth] = useState([0]);
    const [weeksOfMonth, setWeeksOfMonth] = useState([0]);
    const [monthsOfYear, setmonthsOfYear] = useState([0]);
    const [year, setYear] = useState(0);
    const [recurrenceDates, setRecurrenceDates] = useState([0]);
    const [startTime, setStartTime] = useState('');
    const [endTime, setEndTime] = useState('');

    const handleIntervalChange = (event) => {
        setInterval(event.target.value);
    };

    const handleDaysOfWeekChange = (event) => {
        setDaysOfWeek(event.target.value);
    };

    const handleStartTimeChange = (event) => {
        setStartTime(event.target.value);
    };

    const handleEndTimeChange = (event) => {
        setEndTime(event.target.value);
    };

    const handleSubmit = (event) => {
        event.preventDefault();
        const rec_rule: ReccurencyRule = {
            frequency: frequency,
            interval: interval,
            daysOfWeek: daysOfWeek,
            daysOfMonth: daysOfMonth,
            weeksOfMonth: weeksOfMonth,
            monthsOfYear: monthsOfYear,
            year: year,
            recurrenceDates: {
                startTime: startTime,
                endTime: endTime,
            },
        };
        console.log(rec_rule);
        setShowReccurenceModal(false);
        //save? and return to detailsModal
    };

    return (
        <div className="my-14 w-full fixed h-screen flex justify-center items-center rounded-xl z-50">
            <div className="bg-white my-3 fixed w-4/12 h-fit rounded-xl shadow-2xl p-2 pt-4">
                <form
                    onSubmit={handleSubmit}
                    className="grid grid-cols-1/6 items-end gap-y-3.5"
                >
                    <FormControl className="row-span-1 flex items-center">
                        <span className="row-span-1 flex">Interval</span>
                        <Select
                            labelId="interval-label"
                            id="interval"
                            value={interval}
                            onChange={handleIntervalChange}
                            className="row-span-1"
                        >
                            <MenuItem value={1}>Every day</MenuItem>
                            <MenuItem value={7}>Every week</MenuItem>
                            <MenuItem value={30}>Every month</MenuItem>
                            <MenuItem value={365}>Every year</MenuItem>
                        </Select>
                    </FormControl>
                    <FormControl className="row-span-1">
                        <div className="flex flex-col">
                            <span className="text-gray-500 text-ms font-bold">Days of week:</span>
                            <div className="flex flex-wrap justify-center">
                                {['Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa', 'Su'].map((day) => (
                                    <div
                                        key={day}
                                        className={`h-8 w-8 flex items-center justify-center rounded-full bg-gray-300 hover:bg-gray-400 cursor-pointer mr-2 mb-2
                                            ${daysOfWeek.includes(day) ? 'bg-orange-500 text-white' : ''}`}
                                        onClick={() => setDaysOfWeek(prevDays => prevDays.includes(day) ? prevDays.filter(item => item !== day) : [...prevDays, day])}
                                    >
                                        {day}
                                    </div>
                                ))}
                            </div>
                        </div>
                    </FormControl>

                    <TextField
                        id="start-time"
                        label="Start time"
                        type="time"
                        value={startTime}
                        onChange={handleStartTimeChange}
                        className={"row-span-1 flex"}
                        InputLabelProps={{
                            shrink: true,
                        }}
                    />
                    <TextField
                        id="end-time"
                        label="End time"
                        type="time"
                        value={endTime}
                        onChange={handleEndTimeChange}
                        className={"row-span-1 flex"}
                        InputLabelProps={{
                            shrink: true,
                        }}
                    />
                    <div className="row-span-1 flex items-center justify-end gap-3">
                        <button
                            className="bg-gray-300 hover:bg-black/60 px-6 py-2 rounded text-white
                                            hover:scale-105 transition ease-out duration-200 transform"
                            type="button"
                            onClick={() => setShowReccurenceModal(false)}
                        >
                            <span className="text-black/60 font-semibold text-ms">Cancel</span>
                        </button>
                        <button
                            className="bg-orange-300 hover:bg-black/60 px-6 py-2 rounded text-white
                                            hover:scale-105 transition ease-out duration-200 transform"
                            type="submit"
                            onClick={handleSubmit}
                        >
                            <span className="text-white-400 font-semibold text-ms">Done</span>
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default ReccurecyRuleModal;