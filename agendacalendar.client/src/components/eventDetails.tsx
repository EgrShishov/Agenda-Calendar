import {useState, useContext} from "react";
import {Event} from '../models/eventModel.ts';
import {any} from "prop-types";
import GlobalContext from "../context/globalContext.ts";
import {EventService} from "../services/eventService.ts";


const EventDetails = () => {
    const {selectedEvent, setSelectedEvent, setShowEventDetails} = useContext(GlobalContext);
    const [title, setTitle] = useState( selectedEvent ? selectedEvent.title : '');
    const [description, setDescription] = useState( selectedEvent ? selectedEvent.title : '');
    const [startTime, setStartTime] = useState( selectedEvent ? selectedEvent.start : '');
    const [endTime, setEndTime] = useState( selectedEvent ? selectedEvent.end : '');
    const [location, setLocation] = useState(selectedEvent? selectedEvent.location : '');

    const eventService = new EventService();

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        const calendarEvent: Event = {
            title: title,
            description: description,
            start: new Date(startTime),
            end: new Date(endTime),
            authorId: 13,
            location: location,
            rec_rule: any
        };
        const calendarId = 18;
        const userId = 13;
        const eventId = 1;
        if(selectedEvent){
            const response = await eventService.createEvent(calendarEvent, calendarId, userId);
            console.log(response);
        } else {
            const response = await eventService.editEvent(calendarEvent, eventId, userId);
            console.log(response);
        }
        setShowEventDetails(false);
    };

    const onDeleteHandle = async () => {
        const eventId = 1;
        const response = await eventService.deleteEvent(eventId);
        setShowEventDetails(false);
        console.log(response);
    };

    return (
        <div className="h-screen w-full fixed left-0 top-0 flex justify-center items-center">
            <form className="bg-white rounded-lg shadow-2xl w-1/4">
                <header className="bg-gray-100 px-4 py-2 flex justify-between items-center">
                      <span className="material-icons-outlined text-gray-400">
                        drag_handle
                      </span>
                        <div>
                            {selectedEvent && (
                                <span
                                    onClick={onDeleteHandle}
                                    className="material-icons-outlined text-gray-400 cursor-pointer"
                                >
                                delete
                                </span>
                            )}
                            <button onClick={() => setShowEventDetails(false)}>
                              <span className="material-icons-outlined text-gray-400">
                                close
                              </span>
                            </button>
                        </div>
                </header>
                <div className="p-3">
                    <div className="grid grid-cols-1/5 items-end gap-y-7">
                        <div></div>
                        <input
                            type="text"
                            name="title"
                            placeholder="Add title"
                            value={title}
                            required
                            className="pt-3 border-0 text-gray-600 text-xl font-semibold pb-2 w-full border-b-2 border-gray-200 focus:outline-none focus:ring-0 focus:border-blue-500"
                            onChange={(e) => setTitle(e.target.value)}
                        />
                        <span className="material-icons-outlined text-gray-400">
                            schedule
                        </span>
                        <p className="text-gary-600 text-xl font-semibold">
                            Start time
                        </p>
                        <p>{}</p>
                        <span className="material-icons-outlined text-gray-400">
                            schedule
                        </span>
                        <p className="text-gary-600 text-xl font-semibold">
                            End time
                        </p>
                        <p>{}</p>
                        <span className="material-icons-outlined text-gray-400">
                            segment
                        </span>
                        <input
                            type="text"
                            name="description"
                            placeholder="Add a description"
                            value={description}
                            required
                            className="pt-3 border-0 text-gray-600 pb-2 w-full border-b-2 border-gray-200 focus:outline-none focus:ring-0 focus:border-blue-500"
                            onChange={(e) => setDescription(e.target.value)}
                        />
                        <span className="material-icons-outlined text-gray-400">
                            bookmark_border
                        </span>
                        <div className="flex gap-x-2">
                            {labelsClasses.map((lblClass, i) => (
                                <span
                                    key={i}
                                    onClick={() => setSelectedLabel(lblClass)}
                                    className={`bg-${lblClass}-500 w-6 h-6 rounded-full flex items-center justify-center cursor-pointer`}
                                >
                                    {selectedLabel === lblClass && (
                                        <span className="material-icons-outlined text-white text-sm">
                                            check
                                        </span>
                                    )}
                                </span>
                            ))}
                        </div>
                    </div>
                </div>
                <footer className="flex justify-end border-t p-3 mt-5">
                    <button
                        type="submit"
                        onClick={handleSubmit}
                        className="bg-blue-500 hover:bg-blue-600 px-6 py-2 rounded text-white"
                    >
                        Save
                    </button>
                </footer>
            </form>
        </div>
    );
};

export default EventDetails;