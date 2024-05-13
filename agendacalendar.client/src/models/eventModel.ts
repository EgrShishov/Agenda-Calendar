
export interface Event {
    title: string,
    description: string,
    location: string,
    startTime: Date,
    endTime: Date,
    recurrenceRule: RecurrenceRule,
}

export interface RecurrenceRule{
    freq: string,
    interval: number,
    byweekday: string[],
    dtstart: string,
    until: string,
}