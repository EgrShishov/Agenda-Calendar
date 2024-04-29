import React, {useState} from 'react';
import GlobalContext from './globalContext.ts';

export default function ContextWrapper(props){
    const [calendarsList, setCalendarsList] = useState([]);


    return (
        <GlobalContext.Provider
            value={{
                calendarsList: calendarsList,
                setCalendarsList: setCalendarsList
            }}
        >
            { props.children }
        </GlobalContext.Provider>
    );
}