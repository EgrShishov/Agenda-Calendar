import { BrowserRouter as Router, Route, Routes} from 'react-router-dom';
import Home from "./pages/home.tsx";
import MainCalendar from "./pages/mainCalendar.tsx";
import Auth from "./pages/auth.tsx";

function App() {
    return(
        <Router>
            <Routes>
                <Route path="/" Component={Home} />
                <Route path="/u" Component={MainCalendar} />
                <Route path="/auth" Component={Auth} />
                {/*<Route path="/account" component={Account} />*/}
                {/*<Route component={NotFound} />*/}
            </Routes>
        </Router>
    )
}

export default App
