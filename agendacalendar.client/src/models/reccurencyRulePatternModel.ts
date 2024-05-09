
export interface ReccurencyRule {
    freq: string,
    interval: number,
    byweekday: string[],
    dtstart: Date,
    until: Date
}
