import {GoogleLogin} from '@react-oauth/google';
import {useNavigate} from "react-router-dom";
import {useCookies} from "react-cookie";
import cover from "../../public/agendaGreetings.jpeg";
import {GoogleAuthService} from "../services/googleAuthService.ts";
import {UserService} from "../services/userService.ts";
import {useState} from "react";

const Auth = () => {
    const Redirect = useNavigate()
    const [cookies, setCookie] = useCookies(['jwt'])
    const googleAuthService = new GoogleAuthService();
    const userService = new UserService();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');


    const handleLogin = async () => {
        const response = await userService.login({
            email: email,
            password: password
        });
        if(response){
            Redirect('/u');
        }
    };

    const handleSignup = async () => {
        const response = await userService.register({
            username: 'test123', //handle that userName is already exist
            birthdayDate: "2024-05-01T12:02:08.823Z",
            email: email, //handle that email is already exist
            password: password //validate stretgh of password
        });
        if(response){
            Redirect('/u');
        }
    };

    const handleOnSuccess = async (credentialResponse) => {
        const result = await googleAuthService.auth(credentialResponse.credential)
        if(result) {
            setCookie('jwt', result);
            Redirect('/u');
        }
    };

    const handleOnError = () => {
        console.error('Login Failed');
    };

    return(
        <div className="w-full h-screen flex items-start bg-[#FD6F41]">

            <div className="relative w-1/2 h-full flex flex-col">
                <div className="absolute top-[7%] left-[10%] flex flex-col items-center justify-center">
                    <h1 className="text-4xl text-white font-bold">Agenda Calendar <span className="text-3xl text-white font-light"> - where every second counts.</span>
                    </h1>
                </div>
                <img src={cover} className="w-full h-screen object-cover"/>
            </div>

            <div className="w-1/2 h-full flex flex-col items-center justify-center">
                <div className="bg-[#F5F5F5] p-20 rounded-2xl flex flex-col items-center justify-between">
                    <div className="w-full flex flex-col max-w-[400px]">
                        <h3 className="text-3xl font-bold mb-4">Welcome back!</h3>
                        <p className="text-sm mb-2">To continue work with calendar, you need to login </p>

                        <div className="w-full flex flex-col">
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
                        </div>

                        <div className="w-full flex items-center justify-between py-2">
                            <p className="text-sm cursor-pointer underline underline-offset-2">Forgot password?</p>
                        </div>

                        <div className="w-full flex flex-col my-4">
                            <button
                                className="w-full my-2 bg-[#060606] text-white font-semibold rounded-md p-3 text-center flex items-center justify-center
                                transition ease-in-out delay-150 hover:-translate-y-1 hover:scale-105 hover:bg-orange-400
                                hover:text-black duration-300"
                                onClick={handleLogin}>
                                Log in
                            </button>
                            <button
                                className="w-full my-2 bg-white border-2 border-black text-orange-400 font-semibold rounded-md p-3 text-center flex items-center justify-center
                                transition ease-in-out delay-150 hover:-translate-y-1 hover:scale-105 hover:bg-black
                                hover:text-blue-300 duration-300"
                                onClick={handleSignup}>
                                Sign up
                            </button>
                        </div>

                        <div className="w-full flex items-center my-3 justify-center relative">
                            <div className="w-full h-[1px] bg-black"/>
                            <p className="text-lg absolute text-black/80 bg-[#f5f5f5]">or</p>
                        </div>

                        <div className="w-full my-2 flex items-center justify-center">
                            <GoogleLogin
                                onSuccess={handleOnSuccess}
                                onError={handleOnError}
                            />
                        </div>
                    </div>

                    <div className="w-full flex items-center justify-center">
                        <p className="text-sm font-normal text-gray-400">Dont have a account? <span
                            className="font-semibold underline inderline-offset-2 cursor-pointer">Sign up right now!</span>
                        </p>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default Auth;