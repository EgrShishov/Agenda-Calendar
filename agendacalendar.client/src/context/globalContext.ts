import React from 'react';

const GlobalContext = React.createContext({
    calendarRef: null,
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
    labelsClasses: [],
});

export default GlobalContext;