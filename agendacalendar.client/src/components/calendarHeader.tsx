import logo from '../../public/agendaLogo.png'
import GlobalContext from "../context/globalContext.ts";
import React, {useContext, useEffect, useState} from "react";


const CalendarHeader = () => {
    const {calendarRef} = useContext(GlobalContext);

    const [title, setTitle] = useState('');
    const [currentView, setCurrentView] = useState('dayGridMonth');

    useEffect(() => {
        const updateCurrentView = () => {
            setCurrentView(calendarRef.current.getApi().view.type);
        };

        if (calendarRef && calendarRef.current) {
            calendarRef.current.getApi().on('viewDidMount', updateCurrentView);
            return () => {
                calendarRef.current.getApi().off('viewDidMount', updateCurrentView);
            };
        }
    }, [calendarRef]);

    useEffect(() => {
        if (calendarRef && calendarRef.current) {
            setTitle(calendarRef.current.getApi().view.title);
        }
    });

    const handleTodayClick = () => {
        calendarRef.current.getApi().today();
    };

    const handlePrevClick = () => {
        calendarRef.current.getApi().prev();
    };

    const handleNextClick = () => {
        calendarRef.current.getApi().next();
    };

    const handleViewClick = (view) => {
        setCurrentView(view);
        calendarRef.current.getApi().changeView(view);
    };

    return (
        <React.Fragment>
            { calendarRef ? (
                <header className="px-4 py-2 flex items-center border border-gray-200 rounded-md shadow-sm">
                    <div className="flex items-center">
                        <img src={logo} alt={"calendar"} className="mr-4 w-8 h-8"/>
                        <p className="text-2xl font-bold text-orange-400 mr-2">
                            Agenda
                        </p>
                        <p className="text-2xl font-bold text-gray-400 mr-10">
                            Calendar
                        </p>
                    </div>
                    <button
                        onClick={handleTodayClick}
                        className="border rounded py-2 px-4 mr-5"
                    >
                        Today
                    </button>
                    <button
                        onClick={handlePrevClick}
                    >
                    <span className="material-icons-outlined cursor-pointer text-gray-600 mx-2">
                      chevron_left
                    </span>
                    </button>
                    <button
                        onClick={handleNextClick}
                    >
                    <span className="material-icons-outlined cursor-pointer text-gray-600 mx-2">
                        chevron_right
                    </span>
                    <span
                        className="text-black/60 text-xl mx-3 "
                    >
                        {title}
                    </span>
                    </button>
                    <div
                        className="flex items-center gap-x-2 ml-4 mr-4"
                    >
                        <button
                            className={`border rounded py-2 px-4 ${currentView === 'dayGridMonth' ? 'bg-orange-400' : ''}`}
                            onClick={() => handleViewClick('dayGridMonth')}
                        >
                            Month
                        </button>
                        <button
                            className={`border rounded py-2 px-4 ${currentView === 'timeGridWeek' ? 'bg-orange-400' : ''}`}
                            onClick={() => handleViewClick('timeGridWeek')}
                        >
                            Week
                        </button>
                        <button
                            className={`border rounded py-2 px-4 ${currentView === 'timeGridDay' ? 'bg-orange-400' : ''}`}
                            onClick={() => handleViewClick('timeGridDay')}
                        >
                            Day
                        </button>
                    </div>
                    <button>
                        <span className="material-icons cursor-pointer text-orange-400 mx-2">
                            settings
                        </span>
                    </button>
                </header>
            ) : (
                <div>Loading</div>
            )}
        </React.Fragment>
    );
};

export default CalendarHeader;