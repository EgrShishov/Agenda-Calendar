import { environment } from "../environments/environment.development.ts";
import { CalendarModel } from "../models/calendarModel.ts";

export class CalendarService{
    baseUrl = environment.apiUrl;

    constructor() {

    }

    async getCalendars(userId: number){
        const response = await fetch(`${this.baseUrl}calendar/u?userId=${userId}`);
        if(response.ok){
            const data = await response.json();
            return data;
        }
    }

    async createCalendar(calendarRequest: CalendarModel, authorId: number){
        const response = await fetch(`${this.baseUrl}calendar/create?id=${authorId}`,{
            method: 'POST',
            headers: {
                'Content-type': 'application/json'
            },
            body: JSON.stringify(calendarRequest)
        });

        if(response.ok) {
            const data = await response.json()
            return data;
        }
    }

    async editCalendar(calendar: CalendarModel, calendarId: number){
        const response = await fetch(`${this.baseUrl}calendar/edit?id=${calendarId}`, {
            method: 'POST',
            headers:{
                'Content-type': 'application/json'
            },
            body: JSON.stringify(calendar)
        });

        if(response.ok) {
            const data = await response.json()
            return data;
        }
    }

    async deleteCalendar(calendarId: number){
        const response = await fetch(this.baseUrl + 'calendar/delete' + calendarId, {
            method: 'GET',
            headers: {

            }
        });
        if(response.ok){
            const data = await response.json();
            return data;
        }
    }

    async exportCalendar(calendarId: number){
        const response = await fetch(this.baseUrl + 'calendar/export/' + calendarId, {
            method: 'GET',
            headers: {

            }
        });

        if(response.ok){
            const data = await response.json();
            return data;
        }
    }

    async importCalendar(file: File, id: number){
        const formData = new FormData();
        formData.append('file', file);

        try{
            const response = await fetch(`${this.baseUrl}/calendar/import?userId=${id}`, {
                method: 'POST',
                body: formData
            });

            if(response.ok){
                const data = await response.json();
                return data;
            }else{
                console.log('import failed')
            }
        } catch(error){
            console.log('Error happend', error);
        }
    }

    async subscribeToCalendar(calendarId: number){
        const response = await fetch(`${this.baseUrl}/calendar/subscribe?id=${calendarId}`,{
            method: 'GET',
            headers:{
                'Content-type': 'application/json'
            }
        });

        if(response.ok){
            const data = await response.json();
            return data;
        }
    }

    async unsubscribeFromCalendar(calendarId: number){
        const response = await fetch(`${this.baseUrl}/calendar/unsubscribe?id=${calendarId}`,{
            method: 'GET',
            headers:{
                'Content-type': 'application/json'
            }
        });

        if(response.ok){
            const data = await response.json();
            return data;
        }
    }
}