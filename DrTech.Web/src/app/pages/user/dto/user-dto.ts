
export class UserDTO {
  id: string;
  email: string;
  address: string;
  fullName: string;
  userId: string;
  latitude: number;
  longitude: string;
  phone: number;
  pinType: string;
  fileName: string;
  memberSince: string;
  userType: string;
  greenPoints:number;
  refuseCount:number;
  reduceCount:number;
  reuseCount:number;
  regiftCount:number;
  reportCount:number;
  recycleCount:number;
  replantCount:number;
  binCount:number;
  userSchools:string;
  userOrganizations:string;
  userBusiness :string;
  totalGP: number;
}

export class AssociationDTO{
  name:string;
  type:string;
}
export class UserRequestDto{
  type:string;
  startDate:any = null;
  endDate:any = null;
}