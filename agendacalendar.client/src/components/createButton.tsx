import {useEffect, useState} from "react";
import {CalendarService} from "../services/calendarService.ts";
import {CalendarModel} from "../models/calendarModel.ts";

const CreateButton = () => {
    const [isModalOpen, setIsModalOpen] = useState(false)
    const calendarService = new CalendarService();
    const openModal = () => {
        setIsModalOpen(true);
    };

    const closeModal = () => {
        setIsModalOpen(false);
    };

    useEffect(() => {
        const createCalendar = async () => {
            const calendar: CalendarModel = {
                title: 'New Calendar',
                calendarDescription: 'Calendar description',
            };
            const response = await calendarService.createCalendar(calendar, 13);
            if(response.ok){
                const data = await response.json();
                return data;
            }
        };
        createCalendar();
    }, [CalendarService]);

    return (
        <button
            onClick={openModal}
            className="text-orange-400 font-bold border p-2 rounded-full flex items-center shadow-md hover:shadow-2xl bg-white">
            <span className="material-icons-outlined w-7 h-7">
                add
            </span>
            <span className="pl-3 pr-7"> Create</span>
        </button>
    );
};

export default CreateButton;
