import {useState} from "react";
import { makeStyles } from '@mui/styles';
import {
    TextField,
    Select,
    MenuItem,
    InputLabel,
    FormControl,
    Button,
    Modal
} from '@mui/material';

const useStyles = makeStyles((theme) => ({
        // formControl: {
        //     margin: theme.spacing(1),
        //     minWidth: 120,
        // },
}));

const ReccurecyRuleModal = () => {
    const [interval, setInterval] = useState(0);
    const [daysOfWeek, setDaysOfWeek] = useState([0])
    const [startTime, setStartTime] = useState('');
    const [endTime, setEndTime] = useState('');
    const classes = useStyles();

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

        //save? and return to detailsModal
    };

    return (
        <div className="my-14 w-full fixed h-screen flex justify-center items-center rounded-xl">
            <div className="bg-white my-3 fixed w-4/12 h-fit rounded-xl shadow-2xl">
                <form onSubmit={handleSubmit}>
                    <FormControl className="">
                        <InputLabel id="interval-label">Interval</InputLabel>
                        <Select
                            labelId="interval-label"
                            id="interval"
                            value={interval}
                            onChange={handleIntervalChange}
                        >
                            <MenuItem value={1}>Every day</MenuItem>
                            <MenuItem value={7}>Every week</MenuItem>
                            <MenuItem value={30}>Every month</MenuItem>
                            <MenuItem value={365}>Every year</MenuItem>
                        </Select>
                    </FormControl>
                    <FormControl className="">
                        <InputLabel id="days-of-week-label">Days of week</InputLabel>
                        <div>
                            {['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'].map((day) => (
                                <div
                                    key={day}
                                    className={`${classes.circle} ${daysOfWeek.includes(day) ? 'selected' : ''}`}
                                    onClick={() => setDaysOfWeek(prevDays => prevDays.includes(day) ? prevDays.filter(item => item !== day) : [...prevDays, day])}
                                >
                                    {day.slice(0, 3)}
                                </div>
                            ))}
                        </div>
                    </FormControl>
                    <TextField
                        id="start-time"
                        label="Start time"
                        type="time"
                        value={startTime}
                        onChange={handleStartTimeChange}
                        className={""}
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
                        className={""}
                        InputLabelProps={{
                            shrink: true,
                        }}
                    />
                    <div className="flex items-center justify-end">
                        <Button
                            className="bg-orange-400 rounded-2"
                            type="button"
                            variant="contained"
                        >
                            <span className="text-black/60 font-semibold text-ms">Cancel</span>
                        </Button>
                        <Button
                            className="bg-orange-400 rounded-2"
                            type="submit"
                            variant="contained"
                        >
                            <span className="text-orange-400 font-semibold text-ms">Done</span>
                        </Button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default ReccurecyRuleModal;