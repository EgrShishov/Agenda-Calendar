import { ReccurencyRule } from "./reccurencyRulePatternModel.ts";

export interface Event {
    title: string,
    description: string,
    authorId: number,
    start: Date,
    end: Date,
    rec_rule: ReccurencyRule,
    location: string,
}