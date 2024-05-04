import {useNavigate} from "react-router-dom";
import {useEffect, useState} from "react";

const HomeComponent = () => {
    const Redirect = useNavigate();
    const gifsList = [
        '../../public/gifs/gif10.gif',
        '../../public/gifs/gif8.gif',
        '../../public/gifs/gif6.gif',
        '../../public/gifs/gif7.gif',
        '../../public/gifs/gif9.gif',
        '../../public/gifs/gif1.gif',
        '../../public/gifs/gif2.gif',
        '../../public/gifs/gif3.gif',
        '../../public/gifs/gif4.gif',
        '../../public/gifs/gif5.gif',
    ];

    const [curIndex, setCurIndex] = useState(0);
    const [gifElement, setGifElement] = useState(gifsList[curIndex]);

    const ChangeGif = () => {
        const newIndex = (curIndex + 1) % gifsList.length;
        setCurIndex(newIndex)
        setGifElement(gifsList[newIndex]);
    };

    useEffect(() => {
        const intervalId = setInterval(ChangeGif, 2000);
        return () => clearInterval(intervalId);
    }, [curIndex]);

    const onClickHandle = () => Redirect('/auth');

    const [isButtonBigger, setIsButtonBigger] = useState(false);
    useEffect(() => {
        const intervalId = setInterval(() => {
            setIsButtonBigger(prevState => !prevState);
        }, 2000);

        return () => clearInterval(intervalId);
    }, []);

    return (
        <div className="w-full h-screen">
            <img src={gifElement}
                 alt="gifImageOfHourGlasses"
                 className="absolute inset-0 object-cover w-full h-full"
            />

            <div className="absolute inset-0 flex flex-col justify-center items-center">
                <p className="text-gray-400 text-4xl">
                    <span className="text-orange-400 font-bold text-5xl">Agenda </span>
                    - where every second counts.
                </p>
                <p className="text-gray-400 text-3xl my-2 text-center">
                    Powerful instrument that <span className="text-orange-400 text-3xl">helps</span> you
                    <span className="text-orange-400 text-3xl"> manage</span> your time.
                    <br/>
                    Don’t miss a moment, because every minute can unlock new opportunities.
                    <br/>
                    <span className="text-orange-400 my-2 font-bold text-4xl">Agenda Calendar </span>
                    - your <span className="text-orange-400 text-3xl">key</span> to time
                    <span className="text-orange-400 text-3xl"> management</span>,
                    your <span className="text-orange-400 text-3xl">tool</span> for crafting history!
                </p>
                <button
                    className={`text-4xl font-bold my-4 text-orange-400 ${
                        isButtonBigger ? 'animate-pulse' : ''
                    }`}
                    onClick={onClickHandle}>
                    Start our trip together
                </button>
            </div>
        </div>
    );
}

export default HomeComponent;