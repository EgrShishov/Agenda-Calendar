import {GoogleLogin} from '@react-oauth/google';
import {useNavigate} from "react-router-dom";
import {useCookies} from "react-cookie";
import {GoogleAuthService} from "../services/googleAuthService.ts";
import {UserService} from "../services/userService.ts";
import {useContext, useEffect, useState} from "react";
import { enGB } from 'date-fns/locale'
import {DatePicker} from 'react-nice-dates'
import 'react-nice-dates/build/style.css';
import GlobalContext from "../context/globalContext.ts";

const Auth = () => {
    const gifs = [
        '../../public/gifs/clocks3.gif',
        '../../public/gifs/clocks5.gif',
        '../../public/gifs/clocks4.gif',
        '../../public/gifs/clocks6.gif',
        '../../public/gifs/clocks9.gif',
        '../../public/gifs/clocks7.gif',
        '../../public/gifs/clocks10.gif',
        '../../public/gifs/clocks8.gif',
    ]

    const {isAuthenticated, setIsAuthenticated} = useContext(GlobalContext);
    const [curIndex, setCurIndex] = useState(0);
    const [gifElement, setGifElement] = useState(gifs[curIndex]);
    const [selectedTab, setSelectedTab] = useState('login');

    const handleTabChange = (tab) => {
        setSelectedTab(tab);
    };

    const Redirect = useNavigate()
    const [cookies, setCookie] = useCookies(['jwt'])
    const googleAuthService = new GoogleAuthService();
    const userService = new UserService();

    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [passwordConfirmation, setPasswordConfirmation] = useState('');
    const [birthdayDate, setBirthdayDate] = useState('');

    const ChangeGif = () => {
        const newIndex = (curIndex + 1) % gifs.length;
        setCurIndex(newIndex)
        setGifElement(gifs[newIndex]);
    };

    useEffect(() => {
        const intervalId = setInterval(ChangeGif, 2000);
        return () => clearInterval(intervalId);
    }, [curIndex]);

    const handleLogin = async () => {
        const response = await userService.login({
            email: email,
            password: password
        });
        if(response) {
            setIsAuthenticated(true);
            Redirect('/u');
        }
    };

    const handleSignup = async () => {
        //check for passwords, form validation

        const response = await userService.register({
            username: username, //handle that userName is already exist
            birthdayDate: new Date(birthdayDate),
            email: email, //handle that email is already exist
            password: password //validate stretgh of password
        });
        console.log(response);
        if(response){
            setIsAuthenticated(true);
            Redirect('/u');
        }
    };

    const handleOnSuccess = async (credentialResponse) => {
        const result = await googleAuthService.auth(credentialResponse.credential)
        if(result) {
            setIsAuthenticated(true);
            Redirect('/u');
        }
    };

    const handleOnError = () => {
        console.error('Login Failed');
    };

    return(
        <div className="w-full h-screen flex items-start bg-transperent">
            <img src={gifElement}
                 alt="gifImageOfHourGlasses"
                 className="absolute inset-0 object-cover w-full h-full"
            />
            <div className="relative w-1/2 h-full flex flex-col">
                <div className="absolute top-1/2 left-[10%] transform -translate-y-1/2">
                    <h1 className="text-4xl text-white font-bold">
                        <span className="">Agenda Calendar</span>
                        <span className="text-3xl text-white font-light"> - where every second counts.</span>
                    </h1>
                </div>
            </div>

            <div className="w-1/2 h-full flex flex-col items-center justify-center z-50">
                <div className="bg-[#F5F5F5] p-20 rounded-2xl flex flex-col items-center justify-between">
                    <div className="w-full flex flex-col min-w-[310px] max-w-[400px] min-h-[536px] max-h-[536px]">
                    <div className="flex justify-between w-full">
                            <button
                                className={`text-lg font-semibold ${selectedTab == 'login' ? 'text-black border-b-2 border-black' : 'text-gray-500'}`}
                                onClick={() => handleTabChange('login')}>
                                Login
                            </button>
                            <button
                                className={`text-lg font-semibold ${selectedTab == 'signup' ? 'text-black border-b-2 border-black' : 'text-gray-500'}`}
                                onClick={() => handleTabChange('signup')}>
                                Sign Up
                            </button>
                        </div>
                        {selectedTab == 'login' && (
                            <div>
                                <h3 className="my-2 text-3xl font-bold mb-4">Welcome back!</h3>
                                <p className="text-sm mb-2">To continue work with calendar, you need to login </p>
                            </div>
                        )}
                        {selectedTab == 'signup' && (
                            <div>
                                <h3 className="my-2 text-3xl font-bold mb-4">Welcome!</h3>
                                <p className="text-sm mb-2">To begin, please create your account.</p>
                            </div>
                        )}

                        <div className="w-full flex flex-col">
                            {selectedTab == 'signup' && (
                                <div>
                                    <input
                                        type="text"
                                        value={username}
                                        onChange={(e) => setUsername(e.target.value)}
                                        placeholder="Username"
                                        className="w-full text-black py-3 bg-transparent border-b-2 border-orange-500 outline-none focus:outline-none"/>
                                    <div style={{ position: 'relative', maxHeight: '200px'}} className="py-3 border-b-2 border-orange-500">
                                        <DatePicker
                                            date={birthdayDate}
                                            onDateChange={setBirthdayDate}
                                            format={"yyyy-MM-dd"}
                                            locale={enGB}>
                                            {({ inputProps, focused }) => (
                                                <input
                                                    className={'input' + (focused ? ' -focused' : '')}
                                                    {...inputProps}
                                                    style={{ backgroundColor: "transparent"}}
                                                    placeholder="Select your birthday date"
                                                />
                                            )}
                                        </DatePicker>
                                    </div>
                                </div>
                            )}
                            <input
                                type="email"
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                                placeholder="Email"
                                className="w-full text-black py-3 bg-transparent border-b-2 border-orange-500 outline-none focus:outline-none"/>
                            <input
                                type="password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                placeholder="Password"
                                className="w-full text-black py-3 bg-transparent border-b-2 border-orange-500 outline-none focus:outline-none"/>
                            {selectedTab == 'signup' && (
                                <div>
                                    <input
                                        type="password"
                                        value={passwordConfirmation}
                                        onChange={(e) => setPasswordConfirmation(e.target.value)}
                                        placeholder="Confirm password"
                                        className="w-full text-black py-3 bg-transparent border-b-2 border-orange-500 outline-none focus:outline-none"/>
                                </div>
                            )}
                        </div>

                        {selectedTab == 'login' && (
                            <div className="w-full flex items-center justify-between py-2">
                                <p className="text-sm cursor-pointer underline underline-offset-2">Forgot password?</p>
                            </div>
                        )}

                        <div className="w-full flex flex-col my-4">
                            {selectedTab == 'login' && (
                                <button
                                    className="w-full my-2 bg-[#060606] text-white font-semibold rounded-md p-3 text-center flex items-center justify-center
                                transition ease-in-out delay-150 hover:-translate-y-1 hover:scale-105 hover:bg-orange-400
                                hover:text-black duration-300"
                                    onClick={handleLogin}>
                                    Log in
                                </button>
                            )}
                            {selectedTab == 'signup' && (
                                <button
                                    className="w-full my-2 bg-white border-2 border-black text-orange-400 font-semibold rounded-md p-3 text-center flex items-center justify-center
                                transition ease-in-out delay-150 hover:-translate-y-1 hover:scale-105 hover:bg-black
                                hover:text-blue-300 duration-300"
                                    onClick={handleSignup}>
                                    Sign up
                                </button>
                            )}
                        </div>

                        {selectedTab == 'login' && (
                            <div>
                                <div className="w-full flex items-center justify-center relative">
                                    <div className="w-full h-[1px] bg-black"/>
                                    <p className="text-lg absolute text-black/80 bg-[#f5f5f5]">or</p>
                                </div>
                                <div className="w-full my-4 flex items-center justify-center">
                                    <GoogleLogin
                                        onSuccess={handleOnSuccess}
                                        onError={handleOnError}
                                    />
                                </div>
                            </div>
                        )}
                    </div>

                    {selectedTab == 'login' && (
                        <div className="w-full flex items-center justify-center">
                            <p className="text-sm font-normal text-gray-400">Dont have a account? <span
                                onClick={() => setSelectedTab('signup')}
                                className="font-semibold underline inderline-offset-2 cursor-pointer">Sign up right now!</span>
                                    </p>
                                </div>
                    )}
                    {selectedTab == 'signup' && (
                        <div className="w-full flex items-center justify-center">
                            <p onClick={() => setSelectedTab('login')}
                                className="text-sm text-gray-400 font-semibold underline inderline-offset-2 cursor-pointer">
                                Already have an account?
                            </p>
                        </div>
                    )}
                </div>
            </div>
        </div>
    )
}

export default Auth;