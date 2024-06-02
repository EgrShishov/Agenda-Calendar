import React from 'react';

const GlobalContext = React.createContext({
    calendarRef: null,
    showEventDetails: false,
    setShowEventDetails: () => {},
    showCalendarModal: false,
    setShowCalendarModal: () => {},
    showReminderModal: false,
    setShowReminderModal: () => {},
    showWorkingHoursModal: false,
    setShowWorkingHoursModal: () => {},
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
    sharedCalendarsList: [],
    setSharedCalendarsList: () => {},
    labelsClasses: [],
    meetings: [],
    setMeetings : () => {},
    showSuggestModal: false,
    setShowSuggestModal: () => {},
    setUsedColors: () => {},
});

export default GlobalContext;