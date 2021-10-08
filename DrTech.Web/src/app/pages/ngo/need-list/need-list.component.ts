import { Component, OnInit, TemplateRef } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';
import { NgoService } from '../service/ngo-service';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { NeedDTO } from '../dto';
import { DropdownDTO, DonationDropdownsViewModel, DropdownViewModelWithTitle } from '../../../common/dropdown-dto';

@Component({
  selector: 'ngx-need-list',
  templateUrl: './need-list.component.html',
  styles: [`
    nb-card {
      transform: translate3d(0, 0, 0);
    }
  `],
})
export class NgoNeedComponent implements OnInit {

  statusList: Array<DropdownDTO> = new Array<DropdownDTO>();
  _dropdowns: DonationDropdownsViewModel = new DonationDropdownsViewModel();
  _subDropdowns: DropdownViewModelWithTitle = new DropdownViewModelWithTitle();
  _needModel: NeedDTO = new NeedDTO();
  _action: string = "";
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
          title: '<i class="nb-edit"></i>'
        }
      ],
    },
    columns: {
      description: {
        title: 'Description',
        type: 'string',
        filter: false,

      },
      cityDescription: {
        title: 'City',
        type: 'string',
        filter: false,
      },
      //statusDescription: {
      //  title: 'Status',
      //  type: 'string',
      //  filter: false,
      //},
    },
  };

  source: LocalDataSource = new LocalDataSource();

  constructor(public ngoRequest: NgoService, private dialogService: NbDialogService) { }

  ngOnInit() {
    this.LoadData();
    this.ngoRequest.GetDropdownList().subscribe(result => {
      this._dropdowns = result.data;
    });
  }
  LoadData() {
    this.ngoRequest.GetAllNeedList().subscribe(result => {
      if (result.statusCode == 0) {
        this.source.load(result.data);
      }
    });
    this._action = "";
  }
  GetSubType() {
    if (this._needModel.type > 0) {
      this.ngoRequest.GetSubTypeDropdown(this._needModel.type).subscribe(result => {
        if (result.statusCode == 0) {

          if (result.data != null && result.data.donationType != null)
            this._subDropdowns = result.data.donationType;
          else
            this._subDropdowns = new DropdownViewModelWithTitle();
        }
      });
    } else {
      this._subDropdowns = new DropdownViewModelWithTitle();
    }
  }
  updateDescription() {
    if (this._needModel != null) {
      var d = this._dropdowns.cityList.list.filter(p => p.value == this._needModel.city);
      var dd = this._dropdowns.donationType.list.filter(p => p.value == this._needModel.type);

      this._needModel.cityDescription = d[0].description;
      this._needModel.typeDescription = dd[0].description;

      if (this._subDropdowns != null && this._subDropdowns.list != null) {
        var ddd = this._subDropdowns.list.filter(p => p.value == this._needModel.subType);
        this._needModel.subTypeDescription = ddd[0].description;
      }
    }
  }

  updateStatus(ref: NbDialogRef<any>) {
    this.updateDescription();
    if (this._action == "edit") {
      this.ngoRequest.UpdateNeedRequest(this._needModel).subscribe(result => {
        if (result.statusCode == 0) {
          this.LoadData();
          ref.close();
          this._needModel = new NeedDTO(); 
        }
      });
    }
    if (this._action == "add") {
      this.ngoRequest.AddNeedRequest(this._needModel).subscribe(result => {
        if (result.statusCode == 0) {
          this.LoadData();
          ref.close();
          this._needModel = new NeedDTO(); 
        }
      });
    }


  }
  onComplaintAction(dialog: TemplateRef<any>, event: any, action: string) {

    this._action = action;

    if (action == "edit") {
      this._needModel = event.data;
    }
    if (action == "add") {
      this._needModel = new NeedDTO();
    }

    this.GetSubType();

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
        field: 'cityDescription',
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
