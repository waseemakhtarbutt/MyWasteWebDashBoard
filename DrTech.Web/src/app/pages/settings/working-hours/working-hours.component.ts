import { Component, OnInit, Output, EventEmitter, TemplateRef } from '@angular/core';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { compileFilter, SortDescriptor } from '@progress/kendo-data-query';
import { ActivatedRoute, Router } from '@angular/router';
import { SettingsService } from '../settings.service';
@Component({
  selector: 'ngx-working-hours',
  templateUrl: './working-hours.component.html',
  styleUrls: ['./working-hours.component.scss']
})
export class WorkingHoursComponent implements OnInit {
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
    var response = await this.settingsService.GetWorkingHoursList();
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
    var formResponse = await this.settingsService.UpdateWorkingHours(this._viewModel);
    if(formResponse.statusCode == 0){
     this.SuccessMessage ="Time has been updated Successfully!"
    }
  }
}

