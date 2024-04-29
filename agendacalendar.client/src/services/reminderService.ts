import {environment} from "../environments/environment.development.ts";
import {Reminder} from "../models/reminderModel.ts";

export class ReminderService{
    baseUrl = environment.apiUrl;

    constructor() {

    }

    async createReminder(reminder: Reminder, eventId: number){
        const response = await fetch(`${this.baseUrl}/reminder/create?id=${eventId}`,{
            method: 'POST',
            headers:{
                'Content-type': 'application/json'
            },
            body: JSON.stringify(reminder)
        });

        if(response.ok){
            const data = await response.json();
            return data;
        }
    }

    async editReminder(reminder: Reminder, reminderId: number){
        const response = await fetch(`${this.baseUrl}/reminder/edit?id=${reminderId}`,{
           method: 'POST',
           headers: {
               'Content-type': 'application/json'
            },
            body: JSON.stringify(reminder)
        });

        if(response.ok){
            const data = await response.json();
            return data;
        }
    }

    async deleteReminder(reminderId: number){
        const response = await fetch(`${this.baseUrl}/reminder/delete?id=${reminderId}`,{
            method: 'POST',
            headers: {
                'Content-type': 'application/json'
            },
        });

        if(response.ok){
            const data = await response.json();
            return data;
        }
    }
}