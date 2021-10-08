export class EmailDTO {
  id: string;
  emailTo: string;
  emailCC: string;
  emailSubject: string;
  emailBody: string;
  status: string;
  serverMessage: string;
  tryCount: number;
  receiverUserType: string;
}
