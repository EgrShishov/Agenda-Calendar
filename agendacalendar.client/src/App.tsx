import {BrowserRouter as Router, Navigate, Route, Routes, useNavigate} from 'react-router-dom';
import Home from "./pages/home.tsx";
import MainCalendar from "./pages/mainCalendar.tsx";
import Auth from "./pages/auth.tsx";
import Account from "./pages/account.tsx";
import Settings from "./pages/settings.tsx";
import NotFound from "./pages/notFound.tsx";
import EditCalendar from "./pages/editCalendar.tsx";
import PrivateRoute from "./components/privateRoute.tsx";
import {ToastContainer} from "react-toastify";

function App() {

    return(
        <div>
            <Router>
                <Routes>
                    <Route path="/" Component={Home} />
                    <Route path="/u" element={<PrivateRoute Component={MainCalendar}/>} />
                    <Route path="/auth" Component={Auth} />
                    <Route path="/u/account" Component={Account} />
                    <Route path="/u/settings" Component={Settings} />
                    <Route path="/calendar/:id/edit" Component={EditCalendar} />
                    <Route path="*" Component={NotFound} />
                </Routes>
            </Router>
            <ToastContainer />
        </div>
    )
}

export default App
