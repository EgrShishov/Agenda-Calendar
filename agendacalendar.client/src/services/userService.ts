import {environment} from "../environments/environment.development.ts";

export class UserService{
    baseUrl = environment.apiUrl;

    constructor() {

    }

    async register(model: any){
        const response = await fetch(`${this.baseUrl}/account/register`,{
            method: 'POST',
            headers: {
                'Content-type': 'application/json'
            },
            body: JSON.stringify(model)
        });

        if(response.ok){
            const data = await response.json();
            localStorage.setItem('JWT', data['token'])
            return data;
        }
    }

    async login(model: any){
        const response = await fetch(`${this.baseUrl}/account/login`,{
            method: 'POST',
            headers: {
                'Content-type': 'application/json'
            },
            body: JSON.stringify(model)
        });

        if(response.ok){
            const data = await response.json();
            localStorage.setItem('JWT', data['token'])
            return data;
        }
    }

    async logout(){
        localStorage.removeItem('user');
    }

    // function setCurrentUser(user: any){
    //
    // };
}