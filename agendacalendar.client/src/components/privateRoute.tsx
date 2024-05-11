import {Navigate} from "react-router-dom";
import {useCookies} from "react-cookie";

const PrivateRoute = ({ Component }) => {
    const [cookies] = useCookies(['jwt']);
    return cookies.jwt ? <Component /> : <Navigate to="/auth"/>;
};

export default PrivateRoute;