import React, {useContext, useState} from "react";
import GlobalContext from "../context/globalContext.ts";
import {CalendarService} from "../services/calendarService.ts";
import {CalendarModel} from "../models/calendarModel.ts";
import {Button, Menu, MenuItem} from "@mui/material";


const CalendarModal = () => {
    const {
        labelsClasses,
        setShowCalendarModal,
        calendarsList,
        setCalendarsList
    } = useContext(GlobalContext);

    const [selectedTab, setSelectedTab] = useState('new');

    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [participants, setParticipants] = useState([]);
    const [email] = useState('');
    const [selectedFile, setSelectedFile] = useState(null);
    const [selectedColor, setSelectedColor] = useState('');

    const handleFileChanged = (event) => {
        setSelectedFile(event.target.files[0]);
    };

    const calendarService = new CalendarService();
    const handleCreateSubmit = async (e) => {
        e.preventDefault()

        const calendarModel: CalendarModel = {
            title: title,
            calendarDescription: description,
            calendarColor: selectedColor
        };

        const response = await calendarService.createCalendar(calendarModel);
        calendarsList.push({calendar: response, checked: true});
        setCalendarsList(calendarsList);

        setShowCalendarModal(false);
    }

    const handleImportSubmit = async (e) =>{
        e.preventDefault();

        const response = await calendarService.importCalendar(selectedFile);
        calendarsList.push(response);
        setCalendarsList(calendarsList);

        setShowCalendarModal(false);
    };

    const [anchorEl, setAnchorEl] = useState(null);

    const handleClick = (event) => {
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    const handleColorSelection = (color) => {
        setSelectedColor(color);
        setAnchorEl(null);
    };

    const colorRows = [];
    for (let i = 0; i < labelsClasses.length; i += 2) {
        colorRows.push(labelsClasses.slice(i, i + 2));
    }

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
                            <button type="button"
                                    className={`text-lg font-semibold ${selectedTab == 'new' ? 'active text-black border-b-2 border-black' : 'text-gray-500'}`}
                                    onClick={() => setSelectedTab('new')}>Create New
                            </button>
                            <button type="button"
                                    className={`text-lg font-semibold ${selectedTab == 'import' ? 'active text-black border-b-2 border-black' : 'text-gray-500'}`}
                                    onClick={() => setSelectedTab('import')}>Import Calendar
                            </button>
                            <button type="button"
                                    className={`text-lg font-semibold ${selectedTab == 'subscribe' ? 'active text-black border-b-2 border-black' : 'text-gray-500'}`}
                                    onClick={() => setSelectedTab('subscribe')}>Subscribe
                            </button>
                        </div>
                        {selectedTab == 'new' && (
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
                                        email
                                    </span>
                                    <span className="text-gray-600 text-xl mx-3 font-semibold">
                                        {email}
                                    </span>
                                </div>

                                <div className="row-span-1 flex items-center">
                                    <Button
                                        onClick={handleClick}
                                        className="flex items-center border border-gray-300 px-3 py-2 rounded-md text-sm font-medium text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent"
                                    >
                                        <span
                                            className={"text-black/60 text-ms mx-3 font-semibold"}
                                        >
                                              Select a Color
                                        </span>
                                        <svg
                                            className="-mr-1 ml-2 h-5 w-5 text-gray-400"
                                            xmlns="http://www.w3.org/2000/svg"
                                            viewBox="0 0 20 20"
                                            fill="currentColor"
                                            aria-hidden="true"
                                        >
                                            <path
                                                fillRule="evenodd"
                                                d="M6.293 7.293a1 1 0 011.414 0L10 9.586l2.293-2.293a1 1 0 111.414 1.414l-3 3a1 1 0 01-1.414 0l-3-3a1 1 0 010-1.414zM7 5a1 1 0 100-2 1 1 0 000 2z"
                                                clipRule="evenodd"
                                            />
                                        </svg>
                                    </Button>
                                    <Menu
                                        anchorEl={anchorEl}
                                        open={Boolean(anchorEl)}
                                        onClose={handleClose}
                                    >
                                        {labelsClasses.map((color, index) => (
                                            <MenuItem
                                                key={index}
                                                className={`mx-2 w-6 h-6 my-2`}
                                                onClick={() => handleColorSelection(color)}
                                                style={{
                                                    backgroundColor: color,
                                                }}
                                            />
                                        ))}
                                    </Menu>
                                    {selectedColor && (
                                        <span className="text-gray-600 text-ms mx-3 font-semibold">
                                            Selected color:
                                            <span
                                                className={`mx-2 w-6 h-6 rounded-full`}
                                                style={{backgroundColor: selectedColor}}
                                            />
                                        </span>
                                    )}
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
                        {selectedTab == 'import' && (
                            <div className="grid grid-cols-1/4 items-end gap-y-3.5">
                                <label htmlFor="filePicker" className="block mb-2 col-span-1">Choose a calendar
                                    file:</label>
                                <input
                                    id="filePicker"
                                    type="file"
                                    accept=".ics"
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
                        {selectedTab == 'subscribe' && (
                            <div>

                            </div>
                        )}
                    </div>
                </form>
            </div>
        </React.Fragment>
    );
};

export default CalendarModal;