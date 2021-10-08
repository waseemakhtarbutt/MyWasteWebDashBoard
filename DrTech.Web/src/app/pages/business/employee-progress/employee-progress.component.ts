import { Component, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BusinessService } from '../service/business-service';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { compileFilter, CompositeFilterDescriptor, filterBy } from '@progress/kendo-data-query';

@Component({
  selector: 'ngx-employee-progress',
  templateUrl: './employee-progress.component.html',
  styleUrls: ['./employee-progress.component.scss']
})
export class EmployeeProgressComponent implements OnInit {
  listViewModel: any[] = [];
  loading=false;
  public employId = 0;

  public gridView: GridDataResult;
  public pageSize = 8;
  public skip = 0;
  public filter: CompositeFilterDescriptor;
  public employeeName:string = "";

  constructor(public businessService: BusinessService, protected route: ActivatedRoute, protected router: Router, private dialogService: NbDialogService) { }

  async ngOnInit() {
    this.LoadData();
  }

  LoadData() {
    this.loading = true;
  
    this.businessService.GetEmployListByRoleWithEmployeeProgress(false).subscribe(result => {
      if (result.statusCode == 0) {
        this.listViewModel = result.data;

        this.loadItems();
      }
    });

    this.loading = false
  }

  private loadItems(): void {
    if(this.skip == this.listViewModel.length)
        this.skip = this.skip - this.pageSize;
    this.gridView = {
        data: this.listViewModel.slice(this.skip, this.skip + this.pageSize),
        total: this.listViewModel.length
    };
  }
  
  public pageChange(event: PageChangeEvent): void {
    this.skip = event.skip;
    this.loadItems();
  }
  searchGrid(search) {
    const predicate = compileFilter(
      { logic: "or",
       filters: [
        { field: "name", operator: "contains", value: search },
        { field: "businessname", operator: "contains", value: search},
        { field: "department", operator: "contains", value: search},
        { field: "designation", operator: "contains", value: search},
        { field: "role", operator: "contains", value: search},     
        { field: "suboffice", operator: "contains", value: search},   
        { field: "contactno", operator: "contains", value: search},   
        ]});
  
        if(search)
        {
          this.gridView = {
            data: this.listViewModel.filter(predicate),
             total: this.listViewModel.length
         };
        }
        else
        {
          this.gridView = {
            data: this.listViewModel.filter(predicate).slice(this.skip, this.skip + this.pageSize),
             total: this.listViewModel.length
         };         
        }
  }
  public filterChange(filter: CompositeFilterDescriptor): void {
    this.filter = filter;   
    this.gridView = {
        data: filterBy(this.listViewModel, filter),
        total: this.listViewModel.length
    };
  }
}

