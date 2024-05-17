import {environment} from "../environments/environment.development.ts";
import {Reminder} from "../models/reminderModel.ts";
import axios from '../../axios-config.ts';

export class ReminderService{
    baseUrl = environment.apiUrl;

    constructor() {

    }

    async createReminder(reminder: Reminder, eventId:number){
        const response = await axios.post(`${this.baseUrl}reminder/create?eventId=${eventId}`, reminder);
        return response.data;
    }

    async editReminder(reminder: Reminder, reminderId: number){
        const response = await axios.post(`${this.baseUrl}reminder/edit?id=${reminderId}`,reminder);
        return response.data;
    }

    async deleteReminder(reminderId: number){
        const response = await axios.post(`${this.baseUrl}reminder/delete?id=${reminderId}`);
        return response.data;
    }
}