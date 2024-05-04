import {useContext, useState} from "react";
import GlobalContext from "../context/globalContext.ts";
import {ReminderService} from "../services/reminderService.ts";
import {Reminder} from "../models/reminderModel.ts";

const ReminderModal = () => {
    const {setShowReminderModal} = useContext(GlobalContext)
    const reminderService = new ReminderService();

    const [description, setDescription] = useState('');
    const [reminderTime, setReminderTime] = useState('');
    const [email, setEmail] = useState('');
    const [notificationInterval, setNotificationInterval] = useState(0);

    const handleSubmit = async (e) => {
        e.preventDefault();
        const eventId = 1;
        const reminder : Reminder = {
            eventId: eventId,
            description: description,
            email: email,
            reminderTime: new Date(reminderTime),
            notificationInterval: {
                ticks: notificationInterval
            }
        };
        const response = await reminderService.createReminder(reminder);
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
                            <input
                                type="date"
                                name="reminderTime"
                                placeholder="Add time"
                                value={reminderTime}
                                required
                                className="border-0 text-gray-600 text-xl mx-3 font-semibold pb-2
                                w-full border-b-2 border-gray-200 focus:outline-none focus:ring-0 focus:border-blue-500"
                                onChange={(e) => setReminderTime(e.target.value)}
                            />
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
                                className="mx-3 border-b-2 border-gray-300 focus:outline-none focus:border-blue-500 w-full py-2 mt-4"
                                onChange={(e) => setEmail(e.target.value)}
                            />
                        </div>

                        <div className="row-span-1 flex items-center">
                            <span className="material-icons-outlined text-black-65">
                                subtitles
                            </span>
                            <input
                                type="number"
                                name="notifiactionInterval"
                                placeholder="Add time"
                                value={notificationInterval}
                                required
                                className="mx-3 border-0 text-gray-600 text-xl mx-3 font-semibold pb-2
                                w-full border-b-2 border-gray-200 focus:outline-none focus:ring-0 focus:border-blue-500"
                                onChange={(e) => setNotificationInterval(parseInt(e.target.value))}
                            />
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