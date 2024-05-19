import { environment } from "../environments/environment.development.ts";
import { CalendarModel } from "../models/calendarModel.ts";
import axios from '../../axios-config.ts';


export class CalendarService{
    baseUrl = environment.apiUrl;

    constructor() {

    }

    async getCalendars(){
        const response = await axios.get(`${this.baseUrl}calendar/calendars`);
        return response.data;
    }

    async createCalendar(calendarRequest: CalendarModel){
        const response = await axios.post(`${this.baseUrl}calendar/create`, calendarRequest);
        return response.data;
    }

    async editCalendar(calendar: CalendarModel, calendarId: number){
        const response = await axios.post(`${this.baseUrl}calendar/edit?id=${calendarId}`, calendar);
        return response.data;
    }

    async deleteCalendar(calendarId: number){
        const response = await axios.post(this.baseUrl + 'calendar/delete?id=' + calendarId);
        return response.data;
    }

    async exportCalendar(calendarId: number){
        const response = await axios.get(this.baseUrl + 'calendar/export?id=' + calendarId);
        return response.data;
    }

    async importCalendar(file){
        const formData = new FormData();
        formData.append('file', file);
        console.log(formData);
        try{
            const response = await axios.post(`${this.baseUrl}calendar/import`, formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            });
            console.log(response);
            return response.data;
        } catch(error){
            console.log('Error happend', error);
        }
    }

    async subscribeToCalendar(calendarUrl: string){
        const response = await axios.post(`${this.baseUrl}calendar/subscribe?url=${calendarUrl}`);
        return response.data;
    }

    async unsubscribeFromCalendar(calendarId: number){
        const response = await axios.post(`${this.baseUrl}calendar/unsubscribe?id=${calendarId}`);
        return response.data;
    }

    async getShared(){
        const response = await axios.get(`${this.baseUrl}calendar/shared`);
        return response.data;
    }

    async share(calendarId: number){
        const response = await axios.get(`${this.baseUrl}calendar/share?id=${calendarId}`);
        return response.data;
    }
}