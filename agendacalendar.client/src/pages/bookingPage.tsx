import React from "react";
import BookingCalendar from "../components/bookingCalendar.tsx";
import {useParams} from "react-router-dom";

const BookingPage = () => {
    const { email } = useParams();

    return (
        <React.Fragment>
            <BookingCalendar email={email}/>
        </React.Fragment>
    )
};

export default BookingPage;