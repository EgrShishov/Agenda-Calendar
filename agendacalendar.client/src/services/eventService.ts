import {environment} from "../environments/environment.development.ts";
import {Event} from "../models/eventModel.ts";
import axios from "../../axios-config.ts";

export class EventService{
    baseUrl = environment.apiUrl;

    constructor() {

    }

    async getUserEvents(){
        const response = await axios.get(`${this.baseUrl}calendar/u`);
        return response.data;
    }

    async getEvent(eventId: number){
        const response = await axios.get(`${this.baseUrl}events?id=${eventId}`);
        return response.data;
    }

    async createEvent(event: Event, calendarId: number){
        const response =
            await axios.post(`${this.baseUrl}events/create?calendarId=${calendarId}`, event);
        return response.data;
    }

    async editEvent(event: Event, eventId: number, calendarId: number){
        const response =
            await axios.post(`${this.baseUrl}events/edit?eventId=${eventId}&calendarId=${calendarId}`, event);
        return response.data;
    }

    async deleteEvent(eventId: number){
        const response = await axios.post(`${this.baseUrl}events/delete?id=${eventId}`);
        return response.data;
    }

    async getUpcoming(){
        const response = await axios.get(`${this.baseUrl}events/upcoming`);
        return response.data;
    }
}