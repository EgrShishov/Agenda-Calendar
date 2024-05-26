import {environment} from "../environments/environment.development.ts";
import axios from "../../axios-config.ts";

export class BookingService{
    baseUrl = environment.apiUrl;

    async getAvaibaleSlots(email: string){
        const response = await axios.get(`${this.baseUrl}booking/available-slots?email=${email}`);
        return response.data;
    }

    async bookMeeting(bookMeeting, slotId: number){
        const response = await axios.post(`${this.baseUrl}booking/book?slotId=${slotId}`, bookMeeting);
        return response.data;
    }
}