import notFound from '../../public/gifs/notfound.gif';
import {useEffect} from "react";
import {useNavigate} from "react-router-dom";

const NotFound = () =>{
    const Redirect = useNavigate()
    useEffect(() => {
        const intervalId = setInterval(notFound, 2000);
        return () => clearInterval(intervalId);
    }, []);

    return (
        <div className="h-screen w-full relative">
            <img
                src={notFound}
                alt="notFoundGif"
                className="absolute inset-0 object-cover w-full h-full"
            />
            <button
                onClick={() => Redirect('/')}
                className="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 text-white font-bold text-4xl z-50">
                Back to Home
            </button>
        </div>
    );
};

export default NotFound;