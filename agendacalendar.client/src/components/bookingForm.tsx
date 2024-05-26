import {useEffect, useState} from "react";
import {TextField} from "@mui/material";
import {format} from "date-fns";
import {useNavigate} from "react-router-dom";
import {BookingService} from "../services/bookingService.ts";

const BookingForm = ({scheduleDescription, scheduleTitle, bookingDetails, setBookingDetails}) => {
    const [formData, setFormData] = useState({
        firstName: '',
        lastName: '',
        email: '',
        description: '',
    });

    const Redirect = useNavigate();
    const [isSubmitted, setIsSubmitted] = useState(false);
    const [animateIcon, setAnimateIcon] = useState(false);

    const bookingService = new BookingService();

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        console.log(bookingDetails);
        const bookMeeting = async () => {
            const response = await bookingService.bookMeeting(formData, bookingDetails.slotId);

            if(response){
                setIsSubmitted(true);
                setAnimateIcon(true);
            }
        };
        bookMeeting();
    };

    useEffect(() => {
        if (animateIcon) {
            const timer = setTimeout(() => {
                setAnimateIcon(false);
            }, 50);
            return () => clearTimeout(timer);
        }
    }, [animateIcon]);

    return(
        <div>
            {isSubmitted ? (
                <div className="flex flex-col items-center space-y-4">
                    <span
                        className={`material-icons-outlined text-4xl text-green-500 transition-transform 
                        duration-500 ease-out transform ${animateIcon ? 'scale-100' : 'scale-150'}`}
                    >
                        done_all
                    </span>
                    <span className="text-2xl font-bold text-black/60">Meeting successfully booked!</span>
                    <button
                        onClick={() => setBookingDetails(null)}
                        className="hover:bg-black/60 bg-green-400 px-6 py-2 rounded text-black hover:text-white
                        hover:scale-105 transition ease-out duration-200 transform">
                        Got it!
                    </button>
                </div>
            ) : (
                <form onSubmit={handleSubmit} className="flex flex-col space-y-4 p-4">
                    <div className="flex flex-col ">
                         <span
                             className="text-2xl font-bold text-black/60"
                         >
                        {scheduleTitle}
                        </span>
                        <span
                            className="text-xl font-normal text-gray-600"
                        >
                            {scheduleDescription}
                        </span>
                        <span className="">Date: {format(bookingDetails.date, 'dd MMM yyyy')}</span>
                        <span className="">Time: {bookingDetails.time}</span>
                    </div>
                    <hr/>
                    <TextField
                        label="First Name"
                        name="firstName"
                        value={formData.firstName}
                        onChange={handleChange}
                        required
                    />
                    <TextField
                        label="Last Name"
                        name="lastName"
                        value={formData.lastName}
                        onChange={handleChange}
                        required
                    />
                    <TextField
                        label="Email"
                        name="email"
                        value={formData.email}
                        onChange={handleChange}
                        required
                        type="email"
                    />
                    <TextField
                        label="Description"
                        name="description"
                        value={formData.description}
                        onChange={handleChange}
                        multiline
                        rows={4}
                    />
                    <div className="flex flex-row gap-x-3">
                        <button
                            onClick={() => setBookingDetails(null)}
                            className="hover:bg-black/60 bg-white-400 border-2 border-gray-400 px-6 py-2 rounded text-black hover:text-white
                        hover:bg-white-400 hover:scale-105 transition ease-out duration-200 transform"
                        >
                            Cancel
                        </button>
                        <button
                            type="submit"
                            className="hover:bg-black/60 bg-orange-400 px-6 py-2 rounded text-black hover:text-white
                        hover:scale-105 transition ease-out duration-200 transform"
                        >
                            Book
                        </button>
                    </div>
                </form>
            )}
        </div>
    );
};

export default BookingForm;