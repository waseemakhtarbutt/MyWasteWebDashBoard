export class DriverDTO {
    id: number;
    fullName: string;
    firstName:string;
    lastName: string;
    vehicleName:string;
    regNumber:string;
    phone:string;
    fileName:string;
    address:string;
  }
  export class DriverRequestDto{
    startDate: any = null;
    endDate: any = null;
}