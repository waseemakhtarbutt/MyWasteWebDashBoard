import { Component, OnInit, TemplateRef } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';
import { RequestService } from '../service/request-service';
import { CommonService } from '../../../common/service/common-service';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { ReuseDTO } from '../dto';
import { DropdownDTO } from '../../../common/dropdown-dto';
import { LocationLinkComponent, UserDetailLinkComponent } from '../../../common/custom-control';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'ngx-regift-request-approval',
  templateUrl: './regift-request-approval.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
    :host 
    /deep/ 
    i.nb-compose:hover
    { 
      background-color: #91C747 !important;
      border-radius: 50px;
      padding:3px 3px 3px 3px;
      color: #ffffff !important;
    }
  `],
})
export class RegiftRequestApprovalComponent implements OnInit {

  statusList: Array<DropdownDTO> = new Array<DropdownDTO>();
  updatedStatus: number = -1;
  settings = {
    actions: {
      add: false,
      delete: false,
      edit: false,
      position: "right",
      class: 'action-column',
      custom: [
        {
          name: 'onComplaintAction',
          title: '<i class="nb-compose"></i>'
        }
      ],
    },
    columns: {
      description: {
        title: 'Description',
        type: 'string',
        filter: false,

      },
      statusDescription: {
        title: 'Status',
        type: 'string',
        filter: false,
      },
      location: {
        type: 'custom',
        title: "Location",
        renderComponent: LocationLinkComponent,
      },
      userName: {
        type: 'custom',
        title: "User Name",
        renderComponent: UserDetailLinkComponent,
      }
    },
  };

  source: LocalDataSource = new LocalDataSource();
  userId: any;
  constructor(public requestService: RequestService, public commonService: CommonService, private dialogService: NbDialogService, private route: ActivatedRoute) {
    this.userId = route.snapshot.paramMap.get("id");
  }

  ngOnInit() {
    this.LoadData();

    this.commonService.GetStatusList().subscribe(result => {
      this.statusList = result.data;
    });
  }

  LoadData() {
    this.requestService.GetRegiftPenddingApprovalList().subscribe(result => {
      if (result.statusCode == 0) {
        result.data.forEach(p => p.pinType = "reuse");
        this.source.load(result.data);
      }
    });
  }
  updateStatus(status: number, data: ReuseDTO, ref: NbDialogRef<any>) {
    this.requestService.UpdateRegiftApprovalStatus(data.id, status).subscribe(result => {
      if (result.statusCode == 0) {
        this.LoadData();
        ref.close();
      }
      
    });
  }
  onComplaintAction(dialog: TemplateRef<any>, event: any) {
    this.dialogService.open(
      dialog,
      {
        context: event.data,
        closeOnBackdropClick: false,
        closeOnEsc: false,
      });
  }
  onSearch(query: string = '') {

    if (query == '') {
      this.source.reset();
      return;
    }
    this.source.setFilter([
      {
        field: 'description',
        search: query
      }, {
        field: 'latitude',
        search: query
      }, {
        field: 'longitude',
        search: query
      }, {
        field: 'statusDescription',
        search: query
      },
    ], false);
    // second parameter specifying whether to perform 'AND' or 'OR' search
    // (meaning all columns should contain search query or at least one)
    // 'AND' by default, so changing to 'OR' by setting false here
  }
  onDeleteConfirm(event): void {
    if (window.confirm('Are you sure you want to delete?')) {
      event.confirm.resolve();
    } else {
      event.confirm.reject();
    }
  }
}
