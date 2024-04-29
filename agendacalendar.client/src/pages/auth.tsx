import cover from '../../public/agendaGreetings.jpeg'

const Auth = () => {
    return(
        <div className="w-full h-screen flex items-start">

            <div className="relative w-1/2 h-full flex flex-col">
                <div className="absolute top-[7%] left-[10%] flex flex-col items-center justify-center">
                    <h1 className="text-4xl text-white font-bold">Agenda Calendar <span className="text-3xl text-white font-light"> - where every second counts.</span>
                    </h1>
                </div>
                <img src={cover} className="w-full h-full object-cover"/>
            </div>

            <div className="w-1/2 h-full bg-[#F5F5F5] flex flex-col p-20 items-center justify-between">

                <div className="w-full flex flex-col max-w-[400px]">
                    <h3 className="text-3xl font-bold mb-4">Welcome back!</h3>
                    <p className="text-sm mb-2">To continue work with calendar, you need to login </p>

                    <div className="w-full flex flex-col">
                        <input
                            type="email"
                            placeholder="Email"
                            className="w-full text-black py-3 bg-transparent border-b-2 border-orange-500 outline-none focus:outline-none"/>
                        <input
                            type="password"
                            placeholder="Password"
                            className="w-full text-black py-3 bg-transparent border-b-2 border-orange-500 outline-none focus:outline-none"/>
                    </div>

                    <div className="w-full flex items-center justify-between py-2">
                        <p className="text-sm cursor-pointer underline underline-offset-2">Forgot password?</p>
                    </div>

                    <div className="w-full flex flex-col my-4">
                        <button
                            className="w-full my-2 bg-[#060606] text-white font-semibold rounded-md p-3 text-center flex items-center justify-center">
                            Log in
                        </button>
                        <button
                            className="w-full my-2 bg-white border-2 border-black text-orange-400 font-semibold rounded-md p-3 text-center flex items-center justify-center">
                            Sign up
                        </button>
                    </div>

                    <div className="w-full flex items-center my-3 justify-center relative">
                        <div className="w-full h-[1px] bg-black"/>
                        <p className="text-lg absolute text-black/80 bg-[#f5f5f5]">or</p>
                    </div>

                    <button
                        className="gsi-material-button py-3 my-2 w-full flex items-center justify-center bg-white">
                        <div className="gsi-material-button-state"></div>
                        <div className="gsi-material-button-content-wrapper">
                            <div className="gsi-material-button-icon">
                                <svg version="1.1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 48 48"
                                     xmlnsXlink="http://www.w3.org/1999/xlink" className="display-block">
                                    <path fill="#EA4335"
                                          d="M24 9.5c3.54 0 6.71 1.22 9.21 3.6l6.85-6.85C35.9 2.38 30.47 0 24 0 14.62 0 6.51 5.38 2.56 13.22l7.98 6.19C12.43 13.72 17.74 9.5 24 9.5z"></path>
                                    <path fill="#4285F4"
                                          d="M46.98 24.55c0-1.57-.15-3.09-.38-4.55H24v9.02h12.94c-.58 2.96-2.26 5.48-4.78 7.18l7.73 6c4.51-4.18 7.09-10.36 7.09-17.65z"></path>
                                    <path fill="#FBBC05"
                                          d="M10.53 28.59c-.48-1.45-.76-2.99-.76-4.59s.27-3.14.76-4.59l-7.98-6.19C.92 16.46 0 20.12 0 24c0 3.88.92 7.54 2.56 10.78l7.97-6.19z"></path>
                                    <path fill="#34A853"
                                          d="M24 48c6.48 0 11.93-2.13 15.89-5.81l-7.73-6c-2.15 1.45-4.92 2.3-8.16 2.3-6.26 0-11.57-4.22-13.47-9.91l-7.98 6.19C6.51 42.62 14.62 48 24 48z"></path>
                                    <path fill="none" d="M0 0h48v48H0z"></path>
                                </svg>
                            </div>
                            <span className="gsi-material-button-contents">Sign in with Google</span>
                        </div>
                    </button>
                </div>

                <div className="w-full flex items-center justify-center">
                    <p className="text-sm font-normal text-gray-400">Dont have a account? <span
                        className="font-semibold underline inderline-offset-2 cursor-pointer">Sign up right now!</span>
                    </p>
                </div>

            </div>
        </div>
    )
}

export default Auth;