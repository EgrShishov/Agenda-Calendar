import {environment} from "../environments/environment.development.ts";
import axios from '../../axios-config.ts';

export class GoogleAuthService{
    baseUrl = environment.apiUrl;

    constructor() {

    }

    async auth(token: string){
        const response = await axios.get(`${this.baseUrl}account/google-signin?token=${token}`);
        return response.data;
    }
}