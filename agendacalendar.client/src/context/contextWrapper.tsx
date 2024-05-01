import React, {useEffect, useState} from 'react';
import GlobalContext from './globalContext.ts';

export default function ContextWrapper(props){
    const [calendarsList, setCalendarsList] = useState([]);
    const [showEventModal, setShowEventModal] = useState(false);
    const [selectedEvent, setSelectedEvent] = useState(null);

    useEffect(() => {
        if(!showEventModal){
            setSelectedEvent(null);
        }
    }, [showEventModal]);

    return (
        <GlobalContext.Provider
            value={{
                calendarsList: calendarsList,
                setCalendarsList: setCalendarsList,
                selectedEvent: selectedEvent,
                setSelectedEvent: setSelectedEvent,
                setShowEventDetails: setShowEventModal,
            }}
        >
            { props.children }
        </GlobalContext.Provider>
    );
}