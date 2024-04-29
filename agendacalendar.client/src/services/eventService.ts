import {environment} from "../environments/environment.development.ts";
import {Event} from "../models/eventModel.ts";

export class EventService{
    baseUrl = environment.apiUrl;

    constructor() {

    }

    async getEvent(eventId: number){
        const response = await fetch(`${this.baseUrl}/events?id=${eventId}`,{
            method: 'GET',
            headers: {
                'Content-type': 'application/json'
            }
        });

        if(response.ok){
            const data = await response.json();
            return data;
        }
    }

    async createEvent(event: Event, calendarId: number, userId: number){
        const response =
            await fetch(`${this.baseUrl}/events/create?calendarId=${calendarId}&authorId=${userId}`, {
            method: 'POST',
            headers: {
                'Content-type': 'application/json'
            },
            body: JSON.stringify(event)
        });

        if(response.ok){
            const data = await response.json();
            return data;
        }
    }

    async editEvent(event: Event, eventId: number, userId: number){
        const response = await fetch(`${this.baseUrl}/events/edit?eventId=${eventId}&authorId=${userId}}`,{
            method: 'POST',
            headers: {
                'Content-type': 'application/json'
            },
            body: JSON.stringify(event)
        });

        if(response.ok){
            const data = await response.json();
            return data;
        }
    }

    async deleteEvent(eventId: number){
        const response = await fetch(`${this.baseUrl}/events/delete?${eventId}`,{
            method: 'POST',
            headers: {
                'Content-type': 'application/json'
            }
        });

        if(response.ok){
            const data = await response.json();
            return data;
        }
    }

}