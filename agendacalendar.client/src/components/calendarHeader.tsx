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
                <header className="grid grid-cols-8 px-4 py-2 items-center border border-gray-200 rounded-md shadow-sm">
                    <div className="col-span-2 flex items-center">
                        <img src={logo} alt={"calendar"} className="mr-4 w-8 h-8"/>
                        <p className="text-2xl font-workbench font-bold text-orange-400 mr-2">
                            Agenda
                        </p>
                        <p className="text-2xl font-bold text-gray-400 mr-10">
                            Calendar
                        </p>
                    </div>
                    <div className="col-span-3 flex items-center gap-x-2.5">
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
                        </button>
                        <span
                            className="text-black/60 text-2xl mx-5"
                        >
                            {title}
                        </span>
                    </div>

                    <div
                        className="col-span-2 flex items-center gap-x-2.5"
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

                    <div className="col-span-1 flex items-center gap-x-2">
                        <button>
                        <span className="material-icons cursor-pointer text-orange-400 mx-2">
                            settings
                        </span>
                        </button>
                        <p>Account</p>
                    </div>
                </header>
            ) : (
                <div>Loading</div>
            )}
        </React.Fragment>
    );
};

export default CalendarHeader;