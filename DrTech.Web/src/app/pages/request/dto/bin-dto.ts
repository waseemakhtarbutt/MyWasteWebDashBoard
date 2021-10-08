import { BinSubItemDTO } from "./bin-subitem-dto";
import { Time } from "highcharts";
import { CommentsDTO } from "./comments-dto";

export class BinDTO {
  id: number;
  name: string;
  latitude: string;
  longitude: string;
  fileNameTakenByUser: string;
  orderID: string;
  userID: number;
  userName: string;
  userPhone: string;
  userAddress: string;
  statusName: string;
  assignTo: number;
  binSubItems: Array<BinSubItemDTO>;
  deliverTime: Date;
  deliverDate: Date;
  deliveryDate: string;
  gpv: number;
  totalGP: number;
  orderStatusID: number;
  totalPrice: number;
  amountPaid: number;
  price: number;
  comments: string;
  paymentMethod: string;
  buyBinComments:Array<CommentsDTO>;
}
