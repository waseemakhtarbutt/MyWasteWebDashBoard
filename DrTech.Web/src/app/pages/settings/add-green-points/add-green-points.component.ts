import { Component, OnInit, Output, EventEmitter, TemplateRef } from '@angular/core';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { compileFilter, SortDescriptor } from '@progress/kendo-data-query';
import { ActivatedRoute, Router } from '@angular/router';
import { SettingsService } from '../settings.service';

@Component({
  selector: 'ngx-add-green-points',
  templateUrl: './add-green-points.component.html',
  styleUrls: ['./add-green-points.component.scss']
})
export class AddGreenPointsComponent implements OnInit {
  listViewModel: any[] = [];
  organizationBadge : any ;
  loading=false;
  public gridView: GridDataResult;
  public pageSize = 8;
  public skip = 0;
  levelName :string = "";
  public sort: SortDescriptor[] = [{
    field: 'name',
    dir: 'asc'
  }];
  public multiple = false;
  public allowUnsort = true;
  _viewModel: any = {};
  itemslist: any;
  subitemslist: any;
  show: boolean = false;
  donationFlag = false;
  SuccessMessage :string = "";

  constructor(public settingsService: SettingsService, private route: ActivatedRoute, private router: Router) { }

  async ngOnInit() {
    this.loading=true;
    var response = await this.settingsService.GetDefaultGreenPointsList();
    if (response.statusCode == 0) {
      this.listViewModel = response.data;
     this.loadItems();

   }
   this.loading=false;

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
    this.gridView = {
        data: this.listViewModel.slice(this.skip, this.skip + this.pageSize),
        total: this.listViewModel.length
    };
  }
  async onSubmit() {
    this.SuccessMessage = "";
    var formResponse = await this.settingsService.SaveDefaultGreenPoints(this._viewModel);
     if (formResponse.statusCode == 0)
       this.SuccessMessage = "Successfully updated! Thanks";

  }
  getSelectedOptionText(event: Event) {
    let selectElementText = event.target['options']
    [event.target['options'].selectedIndex].text;
    if (selectElementText == "Other") {
      this.donationFlag = true;
      this.subitemslist = null;
    }
    else {
      this._viewModel.typeDescription = "";
      this._viewModel.subTypeDescription = "";
      this.donationFlag = false;

    }

  }
  numberOnly(event): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;

  }

}
