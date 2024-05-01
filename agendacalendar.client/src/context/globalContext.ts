import React from 'react';

const GlobalContext = React.createContext({
    calendarsList: [],
    setCalendarsList: () => {},
    selectedEvent: null,
    setSelectedEvent: () => {},
    showEventDetails: false,
    setShowEventDetails: (showEventDetails: boolean) => {},
    // filteredEvents: [],
});

export default GlobalContext;