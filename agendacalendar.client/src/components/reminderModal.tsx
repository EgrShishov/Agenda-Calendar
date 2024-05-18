import React, {useContext, useState} from "react";
import GlobalContext from "../context/globalContext.ts";
import {ReminderService} from "../services/reminderService.ts";
import {Reminder} from "../models/reminderModel.ts";
import {DatePicker} from "react-nice-dates";
import {enGB} from "date-fns/locale";
import {Menu, TextField} from "@mui/material";
import {makeStyles} from "@mui/styles";
import {isToday} from "date-fns";

const useStyles = makeStyles({
    menu: {
        display: 'flex',
        alignItems: 'center',
    },
    textField: {
        marginRight: 8,
    },
});

const ReminderModal = () => {
    const {setShowReminderModal} = useContext(GlobalContext)
    const reminderService = new ReminderService();

    const [description, setDescription] = useState('');
    const [reminderTime, setReminderTime] = useState('');

    const handleReminderTimeChange = (newTime) => {
        const now = new Date();
        if (newTime && isToday(newTime) && newTime < now) {
            setReminderTime(now);
        } else {
            setReminderTime(newTime);
        }
    };

    const [email, setEmail] = useState('');
    const [notificationInterval, setNotificationInterval] = useState(0);

    const classes = useStyles();
    const [anchorEl, setAnchorEl] = useState(null);
    const [hours, setHours] = useState(0);
    const [minutes, setMinutes] = useState(0);

    const handleClick = (event) => {
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    const handleHoursChange = (event) => {
        setHours(parseInt(event.target.value) || 0);
    };

    const handleMinutesChange = (event) => {
        setMinutes(parseInt(event.target.value) || 0);
    };

    const getNotificationInterval = () => {
        return `Repeat every ${hours.toString().padStart(2, '0')} hours ${minutes.toString().padStart(2, '0')} minutes`;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const eventId = 0;
        setNotificationInterval(hours * 60 + minutes);
        const reminder : Reminder = {
            description: description,
            email: email,
            reminderTime: reminderTime,
            notificationInterval: 10
        };
        const response = await reminderService.createReminder(reminder, eventId);
        setShowReminderModal(false);
        console.log(response);
    };

    return (
        <div className="my-14 w-full fixed h-screen flex justify-center items-center rounded-xl z-50">
            <form className="bg-white my-3 fixed w-4/12 h-fit rounded-xl shadow-2xl">
                <header className="rounded-t-xl bg-orange-300 px-4 py-2.5 flex justify-between items-center">
                      <span className="material-icons-outlined text-black-400">
                        drag_handle
                      </span>
                    <div>
                        <button onClick={() => setShowReminderModal(false)}>
                              <span className="mx-2 material-icons-outlined font-light text-black-65">
                                  close
                              </span>
                        </button>
                    </div>
                </header>
                <div className="p-3">
                    <div className="grid grid-cols-1/4 items-end gap-y-3.5">
                        <div></div>
                        <div className="row-span-1 flex items-center">
                            <span className="material-icons-outlined text-black-65">
                                segment
                            </span>
                            <input
                                type="text"
                                name="description"
                                placeholder="Add description"
                                value={description}
                                required
                                className="mx-3 border-0 text-gray-600 text-ms font-semibold pb-2 w-full border-b-2 border-gray-200 focus:outline-none focus:ring-0 focus:border-blue-500"
                                onChange={(e) => setDescription(e.target.value)}
                            />
                        </div>

                        <div className="row-span-1 flex items-center">
                            <span className="material-icons-outlined text-black-65">
                                calendar_today
                            </span>
                            <div className="mx-3" style={{position: 'relative', maxHeight: '300px'}}>
                                <DatePicker
                                    date={reminderTime}
                                    minimumDate={Date.now()}
                                    onDateChange={handleReminderTimeChange}
                                    format={"yyyy-MM-dd HH:mm"}
                                    locale={enGB}>
                                    {({inputProps, focused}) => (
                                        <input
                                            className={'input' + (focused ? ' -focused' : '')}
                                            {...inputProps}
                                            style={{backgroundColor: "transparent"}}
                                            placeholder="Select reminder time"
                                        />
                                    )}
                                </DatePicker>
                            </div>
                        </div>

                        <div className="row-span-1 flex items-center">
                            <span className="material-icons-outlined text-black-65">
                                email
                            </span>
                            <input
                                type="email"
                                name="email"
                                placeholder="Add email"
                                value={email}
                                required
                                className="mx-3 border-b-2 border-gray-300 focus:outline-none w-full mt-4"
                                onChange={(e) => setEmail(e.target.value)}
                            />
                        </div>

                        <div className="row-span-1 flex items-center">
                            <span className="material-icons-outlined text-black-65">
                                subtitles
                            </span>
                            <div className={"mx-3"}>
                                <button onClick={handleClick}>
                                    {getNotificationInterval()}
                                </button>
                                <Menu
                                    id="notification-interval-menu"
                                    anchorEl={anchorEl}
                                    open={Boolean(anchorEl)}
                                    onClose={handleClose}
                                    className={classes.menu}
                                >
                                    <TextField
                                        type="number"
                                        inputProps={{min: 0, max: 23}}
                                        value={hours}
                                        onChange={handleHoursChange}
                                        className={classes.textField}
                                        label="Hours"
                                        variant="outlined"
                                    />
                                    <TextField
                                        type="number"
                                        inputProps={{min: 0, max: 59}}
                                        value={minutes}
                                        onChange={handleMinutesChange}
                                        className={classes.textField}
                                        label="Minutes"
                                        variant="outlined"
                                    />
                                </Menu>
                            </div>
                        </div>

                        <button
                            type="submit"
                            onClick={handleSubmit}
                            className="bg-orange-300 hover:bg-black/60 px-6 py-2 rounded text-white
                                        hover:scale-105 transition ease-out duration-200 transform"
                        >
                            Add new reminder
                        </button>
                    </div>
                </div>
            </form>
        </div>
    );
};

export default ReminderModal;