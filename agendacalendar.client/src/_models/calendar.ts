import { Reminder } from "./reminder";

export interface Calendar {
    title: string,
    description: string,
    authorId: number,
    events: Event[],
    reminders: Reminder[],
}