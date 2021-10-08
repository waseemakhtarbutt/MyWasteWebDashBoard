import { Component, OnInit, TemplateRef } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { EmailService } from '../service/email-service';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { Router } from '@angular/router';
import { compileFilter, SortDescriptor, orderBy } from '@progress/kendo-data-query';
import { EmailDTO } from '../dto';

@Component({
  selector: 'ngx-notsent',
  templateUrl: './notsent.component.html',
  styleUrls: ['./notsent.component.scss'],
})
export class NotSentComponent implements OnInit {
  emailBodyHTML = "";
  listViewModel: any[] = [];
  loading = false;
  public gridView: GridDataResult;
  public pageSize = 8;
  public skip = 0;
  updatedStatus: number = -1;
  points: number = 0;
  userId: any;
  statusId: any;
  public sort: SortDescriptor[] = [{
    field: 'emailTo',
    dir: 'asc'
  }];
  public multiple = false;
  public allowUnsort = true;
  source: LocalDataSource = new LocalDataSource();
  constructor(public emailService: EmailService, private dialogService: NbDialogService) { }

  ngOnInit() {
    this.LoadData();
  }

  LoadData1() {
    this.emailService.GetNotSentEmailList().subscribe(result => {
      if (result.statusCode == 0) {
        this.source.load(result.data);
      }
    });
  }
  LoadData() {
    this.emailService.GetNotSentEmailList().subscribe(result => {
      if (result.statusCode == 0) {
        let i;

        for (i=0; i<result.data.length; i++)
        {
          if (result.data[i].status == '0')
            result.data[i].status = "Email Pending";
          else
            result.data[i].status = "Email Sent";
        }
        // result.data.forEach(p => p.pinType = "user");
        this.source.load(result.data);
        this.listViewModel = result.data;
        this.loadItems();
      }
    });
  }
  public pageChange(event: PageChangeEvent): void {
    this.skip = event.skip;
    this.loadItems();
  }
  public sortChange(sort: SortDescriptor[]): void {
    this.sort = sort;
    this.loadItems();
}
  private loadItems(): void {
    if (this.skip == this.listViewModel.length)
      this.skip = this.skip - this.pageSize;
    this.gridView = {
      data: orderBy(this.listViewModel.slice(this.skip, this.skip + this.pageSize),this.sort),
      total: this.listViewModel.length
    };

  }
  sendEmail(data, ref: NbDialogRef<any>) {
    this.emailService.SendEmail(data).subscribe(result => {
      if (result.statusCode == 0) {

        this.LoadData();
        ref.close();
      }
    });
  }

  emaldto: EmailDTO

  async onComplaintAction(dialog: TemplateRef<any>, event: any, id: number) {
    var response = await this.emailService.GetEmailById(id);
    if (response.statusCode == 0) {

      this.emaldto = response.data;
      this.emailBodyHTML = this.emaldto.emailBody;
    }

    this.dialogService.open(
      dialog,
      {
        context: this.emaldto,
        closeOnBackdropClick: false,
        closeOnEsc: false,

      });
  }
  searchGrid(search) {

    const predicate = compileFilter(
      {
        logic: "or",
        filters: [
          { field: "emailTo", operator: "contains", value: search },
          { field: "emailSubject", operator: "contains", value: search },
        ]
      });

    if (search) {
      this.gridView = {
        data: this.listViewModel.filter(predicate),
        total: this.listViewModel.length
      };
    }
    else {
      this.gridView = {
        data: this.listViewModel.filter(predicate).slice(this.skip, this.skip + this.pageSize),
        total: this.listViewModel.length
      };
    }
  }
}
