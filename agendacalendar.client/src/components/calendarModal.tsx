import React, {useContext, useState} from "react";
import GlobalContext from "../context/globalContext.ts";
import {CalendarService} from "../services/calendarService.ts";
import {CalendarModel} from "../models/calendarModel.ts";

const CalendarModal = () => {
    const {setShowCalendarModal, calendarsList, setCalendarsList} = useContext(GlobalContext);

    const [activeTab, setActiveTab] = useState('new');

    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [participants, setParticipants] = useState([]);
    const [email] = useState('users email');
    const [selectedFile, setSelectedFile] = useState(null);

    const handleFileChanged = (event) => {
        setSelectedFile(event.target.files[0]);
    };

    const calendarService = new CalendarService();
    const handleCreateSubmit = async (e) => {
        e.preventDefault()
        const calendarModel: CalendarModel = {
            title: title,
            calendarDescription: description
        };
        const response = await calendarService.createCalendar(calendarModel);
        calendarsList.push(response);
        console.log(calendarsList);
        setCalendarsList(calendarsList);
        setShowCalendarModal(false);
    }

    const handleImportSubmit = async (e) =>{
        e.preventDefault();
        const response = await calendarService.importCalendar(selectedFile);
        calendarsList.push(response);
        setCalendarsList(calendarsList);
        console.log(response);
        setShowCalendarModal(false);
    };

    return (
        <React.Fragment>
            <div className="my-14 w-full fixed h-screen flex justify-center items-center rounded-xl z-50">
                <form className="bg-white my-3 fixed w-4/12 h-fit rounded-xl shadow-2xl">
                    <header className="rounded-t-xl bg-orange-300 px-4 py-2.5 flex justify-between items-center">
                      <span className="material-icons-outlined text-black-400">
                        drag_handle
                      </span>
                        <div>
                            <button onClick={() => setShowCalendarModal(false)}>
                              <span className="mx-2 material-icons-outlined font-light text-black-65">
                                  close
                              </span>
                            </button>
                        </div>
                    </header>
                    <div className="p-3">
                        <div className="flex justify-between mb-4">
                            <button type="button" className={`tab-btn ${activeTab == 'new' ? 'active' : ''}`}
                                    onClick={() => setActiveTab('new')}>Create New
                            </button>
                            <button type="button" className={`tab-btn ${activeTab == 'import' ? 'active' : ''}`}
                                    onClick={() => setActiveTab('import')}>Import Calendar
                            </button>
                        </div>
                        {activeTab == 'new' && (
                            <div className="grid grid-cols-1/4 items-end gap-y-3.5">
                                <div></div>
                                <div className="row-span-1 flex items-center">
                            <span className="material-icons-outlined text-black-65">
                                subtitles
                            </span>
                                    <input
                                        type="text"
                                        name="title"
                                        placeholder="Add title"
                                        value={title}
                                        required
                                        className="border-0 text-gray-600 text-xl mx-3 font-semibold pb-2
                                w-full border-b-2 border-gray-200 focus:outline-none focus:ring-0 focus:border-blue-500"
                                        onChange={(e) => setTitle(e.target.value)}
                                    />
                                </div>

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
                                    {email}
                                </span>
                                </div>

                                <button
                                    type="submit"
                                    onClick={handleCreateSubmit}
                                    className="bg-orange-300 hover:bg-black/60 px-6 py-2 rounded text-white
                                        hover:scale-105 transition ease-out duration-200 transform"
                                >
                                    Add new calendar
                                </button>
                            </div>
                        )}
                        {activeTab == 'import' && (
                            <div className="grid grid-cols-1/4 items-end gap-y-3.5">
                                <label htmlFor="filePicker" className="block mb-2 col-span-1">Choose a calendar
                                    file:</label>
                                <input
                                    id="filePicker"
                                    type="file"
                                    accept=".ics" // Указываем тип файлов, которые можно выбрать
                                    onChange={(e) => handleFileChanged(e)}
                                    className="border border-gray-300 p-2 rounded-md col-span-1"
                                />
                                <button
                                    type="submit"
                                    onClick={handleImportSubmit}
                                    className="bg-orange-300 hover:bg-black/60 px-6 py-2 rounded text-white
                                        hover:scale-105 transition ease-out duration-200 transform"
                                >
                                    Import calendar
                                </button>
                            </div>
                        )}
                    </div>
                </form>
            </div>
        </React.Fragment>
    );
};

export default CalendarModal;