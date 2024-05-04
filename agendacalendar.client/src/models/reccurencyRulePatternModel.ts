
export interface ReccurencyRule {
    frequency: number,
    interval: number,
    daysOfWeek: number[],
    daysOfMonth: number[],
    weeksOfMonth: number[],
    monthsOfYear: number[],
    year: number,
    recurrenceDates: ReccurencyDate[],
}

export interface ReccurencyDate{
    startTime: Date,
    endTime: Date,
}