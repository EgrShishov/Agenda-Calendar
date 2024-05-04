import logo from '../../public/agendaLogo.png'
import {useCookies} from "react-cookie";


const CalendarHeader = () => {
    return (
        <header className="px-4 py-2 flex items-center border border-gray-200 rounded-md shadow-sm">
            <img src={logo} alt={"calendar"} className="mr-4 w-8 h-8"/>
            <p className="text-2xl font-bold text-orange-400 mr-2">
                Agenda
            </p>
            <p className="text-2xl font-bold text-gray-400 mr-10">
                Calendar
            </p>
            {/*<button className="border rounded py-2 px-4 mr-5">*/}
            {/*    Today*/}
            {/*</button>*/}
            {/*<button>*/}
            {/*    <span className="material-icons-outlined cursor-pointer text-gray-600 mx-2">*/}
            {/*      chevron_left*/}
            {/*    </span>*/}
            {/*</button>*/}
            {/*<button>*/}
            {/*    <span className="material-icons-outlined cursor-pointer text-gray-600 mx-2">*/}
            {/*        chevron_right*/}
            {/*    </span>*/}
            {/*</button>*/}
            <button>
                <span className="material-icons cursor-pointer text-orange-400 mx-2">
                    settings
                </span>
            </button>
        </header>
    );
};

export default CalendarHeader;