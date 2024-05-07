import axios from 'axios';
import {Cookies} from "react-cookie";

const cookies = new Cookies();

axios.interceptors.request.use(
    config => {
        const jwtCookie = cookies.get('jwt');
        if(jwtCookie){
            config.headers['Authorization'] = 'Bearer ' + jwtCookie;
        }
        return config
    },
    error => {
        Promise.reject(error);
    }
);

axios.interceptors.response.use(
    response => {
        const token = response.data['token'];
        if(token){
            cookies.set('jwt', token, {
                expires: new Date(Date.now() + 60 * 60 * 1000),
            });
        }
        return response;
    },
    function (error){
        return Promise.reject(error)
    }
);


export default axios;