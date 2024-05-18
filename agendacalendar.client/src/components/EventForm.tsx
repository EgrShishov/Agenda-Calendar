import React, { useState, useEffect } from 'react';
import DatePicker from 'react-nice-date';
import { useForm, Controller } from 'react-hook-form';
import * as yup from 'yup';
import { yupResolver } from '@hookform/resolvers/yup';
import 'react-datepicker/dist/react-datepicker.css';

const EventForm = ({ selectedEvent, editingMode, onEditHandler, onDeleteHandle, setShowEventDetails, calendarsList, selectedCalendar, setSelectedCalendar, notifications, reccurenceRule, formatRecurrencePattern, GetCalendarName }) => {
    const [title, setTitle] = useState(selectedEvent ? selectedEvent.title : '');
    const [startTime, setStartTime] = useState(selectedEvent ? new Date(selectedEvent.startTime) : new Date());
    const [endTime, setEndTime] = useState(selectedEvent ? new Date(selectedEvent.endTime) : new Date());
    const [isAllDay, setIsAllDay] = useState(selectedEvent ? selectedEvent.isAllDay : false);
    const [description, setDescription] = useState(selectedEvent ? selectedEvent.description : '');
    const [location, setLocation] = useState(selectedEvent ? selectedEvent.location : '');
    const [selectedItem, setSelectedItem] = useState('');
    const [anchorEl, setAnchorEl] = useState(null);

    const schema = yup.object().shape({
        title: yup.string().required('Title is required'),
        startTime: yup.date().required('Start time is required').typeError('Invalid date'),
        endTime: yup.date().required('End time is required').typeError('Invalid date').min(yup.ref('startTime'), 'End time must be after start time'),
        description: yup.string().required('Description is required'),
        location: yup.string().required('Location is required'),
    });

    const { register, handleSubmit, control, formState: { errors } } = useForm({
        resolver: yupResolver(schema),
    });

    const onSubmit = (data) => {
        console.log(data);
        // Handle form submission
    };

    const handleCalendarChange = (e) => {
        const calendar = calendarsList.find(calendar => calendar.calendar.id === e.target.value);
        setSelectedCalendar(calendar);
    };

    const handleItem = (item) => {
        setSelectedItem(item);
        setAnchorEl(null);
    };

    const handleClick = (event) => {
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    const eventIsAllDay = (e) => {
        setIsAllDay(e.target.checked);
    };

    const startDateChanged = (date) => {
        setStartTime(date);
    };

    const endDateChanged = (date) => {
        setEndTime(date);
    };

    return (
        <div></div>
    );
};

export default EventForm;

