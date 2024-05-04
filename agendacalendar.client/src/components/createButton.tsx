import React, {useContext, useState} from "react";
import GlobalContext from "../context/globalContext.ts";

const CreateButton = () => {
    const { setShowEventDetails, setShowCalendarModal, setShowReminderModal } = useContext(GlobalContext)
    const [showPopover, setShowPopover] = useState(false);
    const [selectedOption, setSelectedOption] = useState(null);

    const onClickHandler = () => {
        setShowEventDetails(true);
    };

    const handleCreateButtonClick = () => {
        setShowPopover(!showPopover);
    };

    const handleOptionSelect = (option) => {
        setSelectedOption(option);
        setShowPopover(false);

        if (option === 'event') {
            setShowEventDetails(true);
            setShowCalendarModal(false);
            setShowReminderModal(false);
        } else if (option === 'calendar') {
            setShowEventDetails(false);
            setShowCalendarModal(true);
            setShowReminderModal(false);
        } else if(option === 'reminder'){
            setShowEventDetails(false);
            setShowCalendarModal(false);
            setShowReminderModal(true);
        }
    };

    return (
        <React.Fragment>
            <button
                onClick={handleCreateButtonClick}
                className="text-orange-400 font-bold border p-2 rounded-full flex items-center shadow-md hover:shadow-2xl bg-white">
            <span className="material-icons-outlined w-7 h-7">
                add
            </span>
                <span className="pl-3 pr-7"> Create</span>
            </button>
            {showPopover && (
                <div className="absolute right-0 top-0 z-50 mt-2 mr-2 bg-white rounded shadow-md">
                    <div className="p-2">
                        <button onClick={() => handleOptionSelect('event')}>Event</button>
                        <br/>
                        <button onClick={() => handleOptionSelect('calendar')}>Calendar</button>
                        <br/>
                        <button onClick={() => handleOptionSelect('reminder')}>Reminder</button>
                    </div>
                </div>
            )}
        </React.Fragment>
    );
};

export default CreateButton;
