import axios from 'axios';
import {Cookies} from "react-cookie";

const cookies = new Cookies();

axios.interceptors.request.use(
    config => {
        const token = cookies.get('jwt')['token'];
        if(token){
            config.headers['Authorization'] = 'Bearer ' + token;
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
            cookies.set('jwt', token);
        }

        return response
    },
    function (error){
        return Promise.reject(error)
    }
);


export default axios;