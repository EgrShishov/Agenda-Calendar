import {environment} from "../environments/environment.development.ts";
import axios from '../../axios-config.ts';

export class UserService{
    baseUrl = environment.apiUrl;

    constructor() {

    }

    async register(model: any){
        const response = await axios.post(`${this.baseUrl}account/register`,
            model);
        return response.data;
    }

    async login(model: any){
        const response = await axios.post(`${this.baseUrl}account/login`, model);
        return response.data;
    }

    async logout(){
        localStorage.removeItem('user');
    }

}