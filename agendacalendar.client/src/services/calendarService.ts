import { environment } from "../environments/environment.development.ts";
import { CalendarModel } from "../models/calendarModel.ts";
import axios from '../../axios-config.ts';


export class CalendarService{
    baseUrl = environment.apiUrl;

    constructor() {

    }

    async getCalendars(userId: number){
        const response = await axios.get(`${this.baseUrl}calendar/calendars?userId=${userId}`, {
            withCredentials: true
        });
        return response.data;
    }

    async createCalendar(calendarRequest: CalendarModel, authorId: number){
        const response = await axios.post(`${this.baseUrl}calendar/create?id=${authorId}`, calendarRequest);
        return response.data;
    }

    async editCalendar(calendar: CalendarModel, calendarId: number){
        const response = await axios.post(`${this.baseUrl}calendar/edit?id=${calendarId}`, calendar);
        return response.data;
    }

    async deleteCalendar(calendarId: number){
        const response = await axios.get(this.baseUrl + 'calendar/delete' + calendarId);
        return response.data;
    }

    async exportCalendar(calendarId: number){
        const response = await axios.get(this.baseUrl + 'calendar/export/' + calendarId);
        return response.data;
    }

    async importCalendar(file: File, id: number){
        const formData = new FormData();
        formData.append('file', file);

        try{
            const response = await axios.post(`${this.baseUrl}/calendar/import?userId=${id}`, formData);
            return response.data;
        } catch(error){
            console.log('Error happend', error);
        }
    }

    async subscribeToCalendar(calendarId: number){
        const response = await axios.get(`${this.baseUrl}/calendar/subscribe?id=${calendarId}`);
        return response.data;
    }

    async unsubscribeFromCalendar(calendarId: number){
        const response = await axios.get(`${this.baseUrl}/calendar/unsubscribe?id=${calendarId}`);
        return response.data;
    }
}