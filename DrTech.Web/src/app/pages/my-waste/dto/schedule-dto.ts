import { Time } from "@angular/common";


export class ScheduleDTO {
    CityID: number;
    AreaID: number;
    AreaName: string;
    DriverName: string;
    DriverID: number;
    Date: Date;
    fTime: Date;
    tTime: Date;
    FromTime: string;
    ToTime: string;
    Day: string;
    Active: boolean;
    Status: string;

}
