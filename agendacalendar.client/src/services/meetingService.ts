import { environment } from "../environments/environment.development.ts";
import axios from '../../axios-config.ts';
import {MeetingModel} from "../models/meetingModel.ts";
import {WorkingHoursModel} from "../models/workingHoursModel.ts";


export class MeetingService{
    baseUrl = environment.apiUrl;

    async getMeetings(){
        const response = await axios.get(`${this.baseUrl}meeting/all`);
        return response.data;
    }

    async createMeeting(meetingRequest: MeetingModel){
        const response = await axios.post(`${this.baseUrl}meeting/create`, meetingRequest);
        return response.data;
    }

    async editMeeting(meeting: MeetingModel, meetingId: number){
        const response = await axios.post(`${this.baseUrl}meeting/edit?id=${meetingId}`, meeting);
        return response.data;
    }

    async deleteMeeting(meetingId: number){
        const response = await axios.post(`${this.baseUrl}meeting/delete?id=${meetingId}`);
        return response.data;
    }

    async acceptMeeting(meetingId: number){
        const response = await axios.post(`${this.baseUrl}meeting/accept?meetingId=${meetingId}`);
        return response.data;
    }

   async declineMeeting(meetingId: number){
       const response = await axios.post(`${this.baseUrl}meeting/decline?meetingId=${meetingId}`);
       return response.data;
   }

   async invite(meetingId: number){
       const response = await axios.post(`${this.baseUrl}meeting/invite?meetingId=${meetingId}`);
       return response.data;
   }

   async getInvitations(){
       const response = await axios.get(`${this.baseUrl}meeting/invitations`);
       return response.data;
   }

   async getUserWorkingHours(){
       const response = await axios.get(`${this.baseUrl}meeting/working_hours`);
       return response.data;
   }

   async setWorkingHours(hours: WorkingHoursModel){
       const response = await axios.post(`${this.baseUrl}meeting/working_hours/set`, hours);
       return response.data;
   }
}