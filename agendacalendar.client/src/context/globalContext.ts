import React from 'react';

const GlobalContext = React.createContext({
    calendarsList: [],
    setCalendarsList: () => {},
    // selectedEvent: null,
    //setSelectedEvent: () => {},
    // filteredEvents: [],
});

export default GlobalContext;