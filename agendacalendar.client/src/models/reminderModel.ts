
export interface Reminder {
    eventId: number,
    description: string,
    email: string
    reminderTime: Date,
    notificationInterval: number
}