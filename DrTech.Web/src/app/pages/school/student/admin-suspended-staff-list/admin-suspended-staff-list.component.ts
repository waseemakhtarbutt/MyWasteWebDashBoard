import { Component, OnInit, TemplateRef} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SchoolService } from '../../service/school.service';
import { GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { NbDialogService, NbDialogRef } from '@nebular/theme';
import { compileFilter, CompositeFilterDescriptor, filterBy, SortDescriptor, orderBy } from '@progress/kendo-data-query';

@Component({
  selector: 'ngx-admin-suspended-staff-list',
  templateUrl: './admin-suspended-staff-list.component.html',
  styleUrls: ['./admin-suspended-staff-list.component.scss']
})

export class AdminSuspendedStaffListComponent implements OnInit {
  listViewModel: any[] = [];
  loading=false;
  public staffId = 0;

  public gridView: GridDataResult;
  public pageSize = 8;
  public skip = 0;
  public filter: CompositeFilterDescriptor;
  public staffName:string = ""
  public sort: SortDescriptor[] = [{
    field: 'name',
    dir: 'asc'
  }];
  public multiple = false;
  public allowUnsort = true;
  constructor(public schoolService: SchoolService, protected route: ActivatedRoute, protected router: Router, private dialogService: NbDialogService) { }

  async ngOnInit() {
    this.LoadData();
  }

  LoadData() {
    this.loading = true;

    this.schoolService.GetStaffListByRole(true).subscribe(result => {
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
        data: orderBy(this.listViewModel.slice(this.skip, this.skip + this.pageSize),this.sort),
        total: this.listViewModel.length
    };
  }

  public pageChange(event: PageChangeEvent): void {
    this.skip = event.skip;
    this.loadItems();
  }
  public sortChange(sort: SortDescriptor[]): void {
    this.sort = sort;
    this.loadItems();
}
  async ActivateSchoolStaff(dialog: NbDialogRef<any>) {
    var response = await this.schoolService.ActivateSchoolStaff(this.staffId);

    if (response.statusCode == 0) {
      dialog.close();
      this.LoadData();
    }
  }

  onComplaintAction(dialog: TemplateRef<any>, event: any, id: any) {
    this.staffName = this.listViewModel.find(x=>x.id==id).name;
    const dialogRef = this.dialogService.open(
      dialog,
      {
        context: event.data,
        closeOnBackdropClick: false,
        closeOnEsc: false,
      });

      this.staffId = id;
  }
  searchGrid(search) {
    const predicate = compileFilter(
      { logic: "or",
       filters: [
        { field: "name", operator: "contains", value: search },
        { field: "schoolname", operator: "contains", value: search },
        { field: "department", operator: "contains", value: search},
        { field: "designation", operator: "contains", value: search},
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
