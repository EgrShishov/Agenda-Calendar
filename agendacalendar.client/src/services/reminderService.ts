import {environment} from "../environments/environment.development.ts";
import {Reminder} from "../models/reminderModel.ts";

export class ReminderService{
    baseUrl = environment.apiUrl;

    constructor() {
    }

    async getUserReminders(userId: number){

    }

    async getEventReminders(eventId: number){

    }

    async createReminder(reminder: Reminder, eventId: number){

    }

    async editReminder(reminder: Reminder){

    }

    async deleteReminder(reminderId: number){

    }
}