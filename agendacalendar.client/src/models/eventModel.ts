import { ReccurencyRule } from "./reccurencyRulePatternModel.ts";

export interface Event {
    title: string,
    description: string,
    startTime: Date,
    endTime: Date,
    recurrenceRule: ReccurencyRule,
    location: string,
}