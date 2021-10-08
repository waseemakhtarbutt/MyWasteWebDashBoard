import { RegiftSubItemDTO } from "./regift-subitem-dto";
import { Time } from "highcharts";
import { CommentsDTO } from "./comments-dto";

export class RegiftDTO {
  id: number;
  description: string;
  latitude: string;
  longitude: string;
  fileNameTakenByUser: string;
  fileNameTakenByDriver: string;
  fileNameTakenByOrg: string;
  collectedPendingConfirmation: boolean;
  deliveredPendingConfirmation: boolean;
  orderID: string;
  userName: string;
  userPhone: string;
  userAddress: string;
  statusName: string;
  assignTo: number;
  regiftSubItems: Array<RegiftSubItemDTO>;
  pickTime: Date;
  pickDate: Date;
  pickupDate: string;
  gpv: number;
  totalGP: number;
  orderStatusID: number;
  updatedDate:Date;
  comments: string;
  regiftComments:Array<object>;
}
