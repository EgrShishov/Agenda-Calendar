import React from 'react';

const GlobalContext = React.createContext({
    isAuthenticated: false,
    setIsAuthenticated: () => {},
    showEventDetails: false,
    setShowEventDetails: () => {},
    showCalendarModal: false,
    setShowCalendarModal: () => {},
    showReminderModal: false,
    setShowReminderModal: () => {},
    selectedEvent: null,
    setSelectedEvent: () => {},
    labels: [],
    setLabels: () => {},
    updateLabel: () => {},
    events: [],
    setEvents: () => {},
    filteredEvents: [],
    calendarsList: [],
    setCalendarsList: () => {},
});

export default GlobalContext;