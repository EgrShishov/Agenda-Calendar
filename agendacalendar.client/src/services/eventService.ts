import {environment} from "../environments/environment.development.ts";
import {Event} from "../models/eventModel.ts";

export class EventService{
    baseUrl = environment.apiUrl;

    constructor() {

    }

    async getEvents(userId: number){

    }

    async createEvent(event: Event, calendarId: number, userId: number){

    }

    async editEvent(event: Event, calendarId: number){

    }

    async deleteEvent(eventId: number){

    }

}